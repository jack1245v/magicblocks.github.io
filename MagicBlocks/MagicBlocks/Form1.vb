'++++++++===============================+++++++++++++ VisualMonsters.Cba.PL +++++++++++===================================

Imports System.Drawing.Drawing2D

Public Class Form1

    'Main Structure of the sliding elements
    Private Structure PlayField
        Dim Col As Integer 'Color as an index in the list colour row
        Dim rect As Rectangle
        Dim x As Integer
        Dim y As Integer
        Dim img As Image
    End Structure

    Private Structure fPatternStr
        Dim col As Integer
        Dim x As Integer
        Dim y As Integer
        Dim img As Image
    End Structure


    'Pattern board
    Private fPattern(3, 3) As Rectangle
    'Main board game board
    Private mFields(4, 4) As Rectangle
    Private mFieldsBool(4, 4) As Boolean

    Dim mBitmap As Bitmap 'Game board bitmap (as a panel background)
    Dim patternBitmap As Bitmap 'Pattern bitmap (as a panel background)

    Dim PatternFieldSize As Integer 'Size of the pattern field
    Dim FieldSize As Integer 'Size of the main board field

    Dim GameStart As Boolean = False 'Variable separating pattern generation from variation of mold size
    Dim GameOverList As New List(Of Integer) 'Checklist for the end of the game

    Dim ColorsList As New List(Of Image) ' List of images (in this case colors)

    Dim PatternCollection As New Collection 'The collection stores the elements of the pattern fields

    'Keeps list of game fields (list, not collection, because list is more flexible)
    Dim FieldCollection As New List(Of PlayField)

    Dim ran As New Random

    Private Sub ColorSet()
        'Takes pictures of moving squares
        ColorsList.Add(My.Resources.red2)
        ColorsList.Add(My.Resources.white2)
        ColorsList.Add(My.Resources.blue)
        ColorsList.Add(My.Resources.amber)
        ColorsList.Add(My.Resources.green)
        ColorsList.Add(My.Resources.yellow)
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'Set the game
        ColorSet()
        SizeChange()
        GameStart = True
    End Sub

    Private Sub SizeChange()
        'Sets the size of the game fields and pattern
        pattern.Size = New Size(Splitter1.Height - 10, Splitter1.Height - 10) 'Size of the board
        pattern.Location = New Point((Splitter1.Width - pattern.Width) / 2, 5) 'The pattern board is always in the middle
        'The size of the game board
        If Me.Width > (Me.Height - Splitter1.Height) Then
            MainBoard.Size = New Size((Me.Height - Splitter1.Height) - 60, (Me.Height - Splitter1.Height) - 60)
        Else
            MainBoard.Size = New Size((Me.Width) - 20, (Me.Width) - 20)
        End If
        'Location in the middle
        MainBoard.Location = New Point((Me.Width - MainBoard.Width) / 2, Splitter1.Height + 10)
        'generate pattern fields and mainboard
        FieldsGenerator()
    End Sub


    Private Sub Form1_Resize(sender As Object, e As EventArgs) Handles MyBase.Resize
        'Enabled only after game preparation
        If GameStart = True Then
            SizeChange()
        End If
    End Sub

    Private Sub FieldsGenerator()

        ' Prepares a clear map Of the fields, their size And location 
        'then clone the board of the game (pattern And main game board) and writes in variables: mBitmap And patternBitmap
        ' which we paint appropriate squares

        Dim MainBitmapRize As New Bitmap(MainBoard.Width, MainBoard.Height)
        Dim PatternBitmapRise As New Bitmap(pattern.Width, pattern.Height)

        Dim g As Graphics = Graphics.FromImage(MainBitmapRize)
        Dim g2 As Graphics = Graphics.FromImage(PatternBitmapRise)

        PatternFieldSize = (pattern.Width / 3) - 3
        FieldSize = (MainBoard.Width / 5) - 5

        If GameStart = False Then
            'Generated in the beginning, it sets the squares, their size and location
            Dim Fcolor As New SolidBrush(Color.FromArgb(60, 240, 240, 240))
            For i As Integer = 0 To 4
                For j As Integer = 0 To 4
                    mFields(i, j) = New Rectangle(10 + (FieldSize * i), 10 + (FieldSize * j), FieldSize - 1, FieldSize - 1)
                    g.FillRectangle(Fcolor, mFields(i, j))

                    If Not j >= 3 Then
                        If Not i >= 3 Then
                            fPattern(i, j) = New Rectangle(5 + (PatternFieldSize * i), 5 + (PatternFieldSize * j), PatternFieldSize - 1, PatternFieldSize - 1)
                            g2.FillRectangle(Fcolor, fPattern(i, j))
                        End If
                    End If
                    'The mFieldsBool array defines which square is empty, in this case, the lower right corner
                    If Not (i = 4 And j = 4) Then
                        mFieldsBool(i, j) = False
                    Else
                        mFieldsBool(i, j) = True
                    End If
                Next
            Next
        Else
            'If this is not the beginning of the game, change only the size and position of the fields, we change the size of the fields in the list
            Dim Fcolor As New SolidBrush(Color.FromArgb(60, 240, 240, 240))
            For i As Integer = 0 To 4
                For j As Integer = 0 To 4
                    'We change the size of the public board
                    mFields(i, j) = New Rectangle(10 + (FieldSize * i), 10 + (FieldSize * j), FieldSize - 1, FieldSize - 1)
                    g.FillRectangle(Fcolor, mFields(i, j))
                    For a As Integer = 0 To FieldCollection.Count - 1
                        If i = FieldCollection(a).x And j = FieldCollection(a).y Then
                            'We assign the changed field to the structure list
                            Dim p As PlayField = FieldCollection(a)
                            p.rect = mFields(i, j)
                            FieldCollection(a) = p
                        End If
                    Next
                Next
            Next
        End If
        'We set the background of the panels
        mBitmap = MainBitmapRize.Clone
        patternBitmap = PatternBitmapRise.Clone
        g.Dispose()
        g2.Dispose()
        'If this is the beginning of the game, you should prepare the pattern and randomly lay out colors
        If GameStart = False Then
            GenerateBoards()
        Else
            'If this is the beginning of the game, you should prepare the pattern and randomly lay out colors
            CompleteFields()
        End If

    End Sub


    Private Sub GenerateBoards()
        ' collection and the list of fields 

        'initiate the structure
        Dim FieldsW As fPatternStr
        Dim polaG As PlayField
        'We create instant bitmaps to avoid damaging the main bitmap
        Dim iPatternBitmap As Bitmap = patternBitmap.Clone
        Dim iMainFields As Bitmap = mBitmap.Clone

        Dim gr_dest As Graphics = Graphics.FromImage(iPatternBitmap)
        Dim gr_dest2 As Graphics = Graphics.FromImage(iMainFields)

        Dim drawList(5) As Integer ' The board will ensure that the colors are no more than 4
        'We start by creating a 3x3 pattern
        For i As Integer = 0 To 2
            For j As Integer = 0 To 2
                'We draw the color
                Dim rand As Integer
                Do
                    rand = ran.Next(0, 6)
                    'If we have three draws of such colors, then we draw again
                    If drawList(rand) + 1 <= 4 Then
                        Exit Do
                    End If
                Loop
                'We fill in the data structure
                FieldsW.x = i
                FieldsW.y = j
                FieldsW.col = rand
                FieldsW.img = cheSize(ColorsList(rand), New Size(PatternFieldSize, PatternFieldSize))

                GameOverList.Add(rand)
                drawList(rand) += 1 'We add a color to the list watching the color amount
                'drw elements
                gr_dest.DrawImage(FieldsW.img, fPattern(i, j).Location.X, fPattern(i, j).Location.Y, FieldsW.img.Width - 1, FieldsW.img.Height - 1)
                PatternCollection.Add(FieldsW) ' We complete the collection
            Next
        Next
        'we generete 5x5 board
        'clean board
        ReDim drawList(5) ' The board will ensure that the colors are no more than 4
        For i As Integer = 0 To 4
            For j As Integer = 0 To 4
                'Complements only false fields, leaving one clean field
                If mFieldsBool(i, j) = False Then
                    'We draw the color
                    Dim rand As Integer
                    Do
                        rand = ran.Next(0, 6)
                        If drawList(rand) + 1 <= 4 Then
                            Exit Do
                        End If
                    Loop
                    'We fill in the data structure
                    polaG.rect = mFields(i, j)
                    polaG.x = i
                    polaG.y = j
                    polaG.Col = rand
                    polaG.img = cheSize(ColorsList(rand), New Size(FieldSize, FieldSize))

                    drawList(rand) += 1 'We add a color to the list watching the color amount
                    'drw elements
                    gr_dest2.DrawImage(polaG.img, mFields(i, j).Location.X, mFields(i, j).Location.Y, polaG.img.Width - 1, polaG.img.Height - 1)
                    FieldCollection.Add(polaG) ' We complete the collection
                End If
            Next
        Next
        'We set new panel backgrounds
        pattern.Image = iPatternBitmap
        MainBoard.Image = iMainFields
    End Sub


    Private Sub CompleteFields()
        'Method of changing game size pattern and board already generated
        Dim iPatternBitmap As Bitmap = patternBitmap.Clone
        Dim iMainBitmap As Bitmap = mBitmap.Clone

        Dim gr_dest As Graphics = Graphics.FromImage(iPatternBitmap)
        Dim gr_dest2 As Graphics = Graphics.FromImage(iMainBitmap)
        'First, we draw the colors Of the pattern On the basis Of the altered size Of the fields,
        'although Nothing Is tampered with here (by the molds of the mold) adds it may be useful to someone who does Not spoil,
        'you can remove it if you want
        For i As Integer = 0 To 2
            For j As Integer = 0 To 2
                For a As Integer = 1 To PatternCollection.Count
                    If i = PatternCollection(a).x And j = PatternCollection(a).y Then
                        Dim myimage As Image = cheSize(PatternCollection(a).img, New Size(PatternFieldSize, PatternFieldSize))
                        gr_dest.DrawImage(myimage, fPattern(i, j).Location.X, fPattern(i, j).Location.Y, myimage.Width - 1, myimage.Height - 1)
                    End If
                Next
            Next
        Next
        pattern.Image = iPatternBitmap 'We set the pattern panel background
        'Then we draw the colors of the main fields of the board based on these field sizes
        For i As Integer = 0 To 4
            For j As Integer = 0 To 4
                If mFieldsBool(i, j) = False Then
                    For a As Integer = 0 To FieldCollection.Count - 1
                        If i = FieldCollection(a).x And j = FieldCollection(a).y Then
                            Dim myimage As Image = cheSize(FieldCollection(a).img, New Size(FieldSize, FieldSize))
                            gr_dest2.DrawImage(myimage, FieldCollection(a).rect.Location.X, FieldCollection(a).rect.Location.Y, myimage.Width - 1, myimage.Height - 1)
                        End If
                    Next
                End If
            Next
        Next
        MainBoard.Image = iMainBitmap
        GameOver()
    End Sub


    Private Sub GameOver()
        'The method checks the end of the game
        Dim YouWin As Boolean = True 'We assume it's over
        Dim CompList As New List(Of Integer) ' List storing the color numbers of the center of the board
        'Get the color numbers in the middle of the board
        For i As Integer = 1 To 3
            For j As Integer = 1 To 3
                For k As Integer = 0 To FieldCollection.Count - 1
                    If FieldCollection(k).x = i And FieldCollection(k).y = j Then
                        CompList.Add(FieldCollection(k).Col)
                    End If
                Next
            Next
        Next
        ' If there is no empty field in the middle, the method goes to comparing the list
        If CompList.Count = GameOverList.Count Then
            For i As Integer = 0 To GameOverList.Count - 1
                If Not GameOverList(i) = CompList(i) Then
                    'The lists are not identical, the end of the comparison
                    YouWin = False
                    Exit For
                End If
            Next
            If YouWin = True Then
                'Lists are the same
                MainBoard.Enabled = False
                MsgBox("Game over!")
            End If
        End If
    End Sub

    Public Shared Function cheSize(ByVal img As Image, ByVal largeness As Size, Optional ByVal keepImageRatio As Boolean = True) As Image

        Dim NewWidth As Integer
        Dim NewHeight As Integer
        'We determine the size of the new image, depending on whether the aspect ratio is to be retained
        If keepImageRatio Then
            Dim OriginalWidth As Integer = img.Width
            Dim OriginalHeight As Integer = img.Height
            Dim PreWidth As Single = CSng(largeness.Width) / CSng(OriginalWidth)
            Dim PreHeight As Single = CSng(largeness.Height) / CSng(OriginalHeight)
            Dim Percent As Single = If(PreHeight < PreWidth,
                PreHeight, PreWidth)
            NewWidth = CInt(OriginalWidth * Percent)
            NewHeight = CInt(OriginalHeight * Percent)
        Else
            NewWidth = largeness.Width
            NewHeight = largeness.Height
        End If

        Dim newImg As Image = New Bitmap(NewWidth, NewHeight)
        'We generate a new image
        Using graphicsHandle As Graphics = Graphics.FromImage(newImg)
            graphicsHandle.InterpolationMode = InterpolationMode.HighQualityBicubic
            graphicsHandle.DrawImage(img, 0, 0, NewWidth, NewHeight)
        End Using
        'Function returns newImg
        Return newImg
    End Function

#Region "Tiles moving"
    Private Sub Form1_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
        'Move with the arrow keys
        Dim Ilocation As Integer
        Dim Jlocation As Integer
        'We are getting the location of the empty field
        For i As Integer = 0 To 4
            For j As Integer = 0 To 4
                If mFieldsBool(i, j) = True Then
                    Ilocation = i
                    Jlocation = j
                End If
            Next
        Next
        'If this is not the end of the game, then we will check which key was used
        If MainBoard.Enabled = True Then
            If e.KeyCode = Keys.Down Then
                'down key
                If Not Jlocation = 0 Then 'If it was less than 0 then it would be outside the array
                    For a As Integer = 0 To FieldCollection.Count - 1
                        'Gets the location of the field above the blank field
                        If Ilocation = FieldCollection(a).x And (Jlocation - 1) = FieldCollection(a).y Then
                            'And change them to new ones
                            Dim p As PlayField = FieldCollection(a)
                            p.y = Jlocation
                            p.rect = mFields(Ilocation, Jlocation)
                            FieldCollection(a) = p 'Overwrites an item in the list
                        End If
                    Next
                    'Changes the field's boolean value
                    mFieldsBool(Ilocation, Jlocation) = False
                    mFieldsBool(Ilocation, Jlocation - 1) = True
                End If
            ElseIf e.KeyCode = Keys.Up Then
                If Not Jlocation = 4 Then 'If it was more than 4 then it would be outside the array
                    For a As Integer = 0 To FieldCollection.Count - 1
                        If Ilocation = FieldCollection(a).x And (Jlocation + 1) = FieldCollection(a).y Then
                            Dim p As PlayField = FieldCollection(a)
                            p.y = Jlocation
                            p.rect = mFields(Ilocation, Jlocation)
                            FieldCollection(a) = p
                        End If
                    Next
                    mFieldsBool(Ilocation, Jlocation) = False
                    mFieldsBool(Ilocation, Jlocation + 1) = True
                End If
            ElseIf e.KeyCode = Keys.Right Then
                If Not Ilocation = 0 Then 'If it was less than 0 then it would be outside the array
                    For a As Integer = 0 To FieldCollection.Count - 1
                        If (Ilocation - 1) = FieldCollection(a).x And Jlocation = FieldCollection(a).y Then
                            Dim p As PlayField = FieldCollection(a)
                            p.x = Ilocation
                            p.rect = mFields(Ilocation, Jlocation)
                            FieldCollection(a) = p

                        End If
                    Next
                    mFieldsBool(Ilocation, Jlocation) = False
                    mFieldsBool(Ilocation - 1, Jlocation) = True
                End If
            ElseIf e.KeyCode = Keys.Left Then
                If Not Ilocation = 4 Then 'If it was more than 4 then it would be outside the array
                    For a As Integer = 0 To FieldCollection.Count - 1
                        If (Ilocation + 1) = FieldCollection(a).x And Jlocation = FieldCollection(a).y Then
                            Dim p As PlayField = FieldCollection(a)
                            p.x = Ilocation
                            p.rect = mFields(Ilocation, Jlocation)
                            FieldCollection(a) = p
                        End If
                    Next
                    mFieldsBool(Ilocation, Jlocation) = False
                    mFieldsBool(Ilocation + 1, Jlocation) = True
                End If
            End If
        End If
        'Redraw new colors
        CompleteFields()
    End Sub

    'mouse click
    Private Sub PlanszaGlowna_MouseMove(sender As Object, e As MouseEventArgs) Handles MainBoard.MouseMove
        'To be able to select a field using the left mouse button, you first need to specify where the mouse cursor is
        Dim Ilocation As Integer
        Dim Jlocation As Integer

        For i As Integer = 0 To 4
            For j As Integer = 0 To 4
                If mFieldsBool(i, j) = True Then
                    Ilocation = i
                    Jlocation = j
                End If
            Next
        Next

        For i As Integer = 0 To FieldCollection.Count - 1
            If (FieldCollection(i).rect.Location.X - 2 <= e.X And (FieldCollection(i).rect.Location.X + FieldCollection(i).rect.Width + 2 >= e.X)) _
                And (FieldCollection(i).rect.Location.Y - 2 <= e.Y And
                (FieldCollection(i).rect.Location.Y + FieldCollection(i).rect.Height + 2 >= e.Y)) Then
                'If the cursor is to the left or to the right of the empty field, the cursor will change to the handle
                If (FieldCollection(i).x - 1 = Ilocation Or FieldCollection(i).x + 1 = Ilocation) And FieldCollection(i).y = Jlocation Then
                    Me.Cursor = Cursors.Hand
                    Exit For
                End If
                'If the cursor is above or below the empty field, the cursor will change the type to the handle
                If (FieldCollection(i).y - 1 = Jlocation Or FieldCollection(i).y + 1 = Jlocation) And FieldCollection(i).x = Ilocation Then
                    Me.Cursor = Cursors.Hand
                    Exit For
                End If
            Else
                'The cursor will remain an arrow or change the arrow type if the above conditions are not met
                Me.Cursor = Cursors.Arrow
            End If
        Next
    End Sub

    Private Sub PlanszaGlowna_MouseClick(sender As Object, e As MouseEventArgs) Handles MainBoard.MouseClick
        Dim Ilocation As Integer
        Dim Jlocation As Integer
        'Gets the location of the empty field
        For i As Integer = 0 To 4
            For j As Integer = 0 To 4
                If mFieldsBool(i, j) = True Then
                    Ilocation = i
                    Jlocation = j
                End If
            Next
        Next
        'Looks at which field is the cursor
        For i As Integer = 0 To FieldCollection.Count - 1
            If (FieldCollection(i).rect.Location.X - 2 <= e.X And (FieldCollection(i).rect.Location.X +
                FieldCollection(i).rect.Width + 2 >= e.X)) And (FieldCollection(i).rect.Location.Y - 2 <= e.Y _
                And (FieldCollection(i).rect.Location.Y + FieldCollection(i).rect.Height + 2 >= e.Y)) Then
                'If you click the box to the right of the empty field
                If FieldCollection(i).x + 1 = Ilocation And FieldCollection(i).y = Jlocation Then
                    'Will retrieve the field structure from the list
                    Dim p As PlayField = FieldCollection(i)
                    'change x location
                    p.x = Ilocation
                    'Will assign a new square
                    p.rect = mFields(Ilocation, Jlocation)
                    'Will overwrite the new structure in the list
                    FieldCollection(i) = p
                    'set new true/false position
                    mFieldsBool(Ilocation, Jlocation) = False
                    mFieldsBool(Ilocation - 1, Jlocation) = True
                End If
                'If you clicked the box to the left of the empty field
                If FieldCollection(i).x - 1 = Ilocation And FieldCollection(i).y = Jlocation Then
                    Dim p As PlayField = FieldCollection(i)
                    p.x = Ilocation
                    p.rect = mFields(Ilocation, Jlocation)
                    FieldCollection(i) = p
                    mFieldsBool(Ilocation, Jlocation) = False
                    mFieldsBool(Ilocation + 1, Jlocation) = True
                End If
                'If you clicked the box is above the empty field
                If FieldCollection(i).y - 1 = Jlocation And FieldCollection(i).x = Ilocation Then
                    Dim p As PlayField = FieldCollection(i)
                    p.y = Jlocation
                    p.rect = mFields(Ilocation, Jlocation)
                    FieldCollection(i) = p

                    mFieldsBool(Ilocation, Jlocation) = False
                    mFieldsBool(Ilocation, Jlocation + 1) = True
                End If
                'If you clicked the box is below the empty field
                If FieldCollection(i).y + 1 = Jlocation And FieldCollection(i).x = Ilocation Then
                    Dim p As PlayField = FieldCollection(i)
                    p.y = Jlocation
                    p.rect = mFields(Ilocation, Jlocation)
                    FieldCollection(i) = p
                    mFieldsBool(Ilocation, Jlocation) = False
                    mFieldsBool(Ilocation, Jlocation - 1) = True
                End If
            End If
        Next
        CompleteFields()
    End Sub

    Private Sub Splitter1_SplitterMoved(sender As Object, e As SplitterEventArgs) Handles Splitter1.SplitterMoved

    End Sub

    Private Sub NewGameButton_Click(sender As Object, e As EventArgs) Handles NewGameButton.Click
        ' Reset the game state and start a new game
        ResetGameState()
    End Sub

    Private Sub ResetGameState()
        ' Reset the game state here, such as resetting the game board and starting a new game
        ' Clear game board
        FieldCollection.Clear()
        ' Reset game state variables
        GameStart = False
        GameOverList.Clear()
        ' Call the methods to initialize the game state
        ColorSet()
        SizeChange()
        GenerateBoards()
        ' Set GameStart to True after initializing the game state
        GameStart = True

    End Sub

    Private Sub Label1_Click(sender As Object, e As EventArgs) Handles Label1.Click

    End Sub

    Dim ss, tt, vv As Integer

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Timer1.Enabled = False
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        vv = 0
        tt = 0
        ss = 0

        Label1.Text = "00:00:00"
        Timer1.Enabled = False
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Timer1.Enabled = True
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        Label1.Text = Format(ss, "00:") & Format(tt, "00:") & Format(vv, "00")
        vv = vv + 1
        If vv > 59 Then
            vv = 0
            tt = tt + 1
        End If
        If tt = 2 Then
            vv = 0
            tt = 0
            Label1.Text = "00:00:00"
            Timer1.Enabled = False
            MessageBox.Show("Timer Ended")
        End If
    End Sub


#End Region

End Class
