<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.Splitter1 = New System.Windows.Forms.Splitter()
        Me.pattern = New System.Windows.Forms.PictureBox()
        Me.MainBoard = New System.Windows.Forms.PictureBox()
        Me.NewGameButton = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.Button3 = New System.Windows.Forms.Button()
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        CType(Me.pattern, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.MainBoard, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Splitter1
        '
        Me.Splitter1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Splitter1.Location = New System.Drawing.Point(0, 0)
        Me.Splitter1.Name = "Splitter1"
        Me.Splitter1.Size = New System.Drawing.Size(659, 188)
        Me.Splitter1.TabIndex = 0
        Me.Splitter1.TabStop = False
        '
        'pattern
        '
        Me.pattern.BackColor = System.Drawing.Color.Black
        Me.pattern.Location = New System.Drawing.Point(243, 12)
        Me.pattern.Name = "pattern"
        Me.pattern.Size = New System.Drawing.Size(282, 152)
        Me.pattern.TabIndex = 1
        Me.pattern.TabStop = False
        '
        'MainBoard
        '
        Me.MainBoard.BackColor = System.Drawing.Color.Black
        Me.MainBoard.Location = New System.Drawing.Point(234, 334)
        Me.MainBoard.Name = "MainBoard"
        Me.MainBoard.Size = New System.Drawing.Size(282, 152)
        Me.MainBoard.TabIndex = 2
        Me.MainBoard.TabStop = False
        '
        'NewGameButton
        '
        Me.NewGameButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.NewGameButton.Location = New System.Drawing.Point(564, 215)
        Me.NewGameButton.Name = "NewGameButton"
        Me.NewGameButton.Size = New System.Drawing.Size(75, 23)
        Me.NewGameButton.TabIndex = 3
        Me.NewGameButton.Text = "New Game"
        Me.NewGameButton.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 24.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(37, 25)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(183, 45)
        Me.Label1.TabIndex = 4
        Me.Label1.Text = "00:00:00"
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(10, 73)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(61, 63)
        Me.Button1.TabIndex = 5
        Me.Button1.Text = "Start"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'Button2
        '
        Me.Button2.Location = New System.Drawing.Point(77, 73)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(53, 63)
        Me.Button2.TabIndex = 6
        Me.Button2.Text = "Stop"
        Me.Button2.UseVisualStyleBackColor = True
        '
        'Button3
        '
        Me.Button3.Location = New System.Drawing.Point(136, 73)
        Me.Button3.Name = "Button3"
        Me.Button3.Size = New System.Drawing.Size(59, 63)
        Me.Button3.TabIndex = 7
        Me.Button3.Text = "Reset"
        Me.Button3.UseVisualStyleBackColor = True
        '
        'Timer1
        '
        Me.Timer1.Interval = 1000
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(659, 630)
        Me.Controls.Add(Me.Button3)
        Me.Controls.Add(Me.Button2)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.NewGameButton)
        Me.Controls.Add(Me.MainBoard)
        Me.Controls.Add(Me.pattern)
        Me.Controls.Add(Me.Splitter1)
        Me.Name = "Form1"
        Me.Text = "Magic Blocks"
        CType(Me.pattern, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.MainBoard, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents Splitter1 As Splitter
    Friend WithEvents pattern As PictureBox
    Friend WithEvents MainBoard As PictureBox
    Friend WithEvents NewGameButton As Button
    Friend WithEvents Label1 As Label
    Friend WithEvents Button1 As Button
    Friend WithEvents Button2 As Button
    Friend WithEvents Button3 As Button
    Friend WithEvents Timer1 As Timer
End Class
