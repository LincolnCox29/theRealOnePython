using System.Runtime.CompilerServices;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Security.Policy;
using theRealOnePythin;

namespace theRealOnePython
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeSettings();

            python.InitializePythonBody(halfFormSize, tileSize);

            apple.AppleSpawn(formSize,tileSize, python);

            InitializeComponent();

            SetStyle(
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.UserPaint, true
                );

            InitializeTimer();

            ClientSize = new Size(formSize, formSize);
        }

        static Dictionary<string, int>? settings;

        char pressedKey = 's';
        bool pressed = false;

        bool pause = false;

        static int milliseconds;
        static int tileSize;
        static int formSize;
        static int halfFormSize;

        Python python = new Python();

        Apple apple = new Apple();

        private System.Windows.Forms.Timer timer;

        private void InitializeTimer()
        {
            timer = new System.Windows.Forms.Timer();
            timer.Interval = milliseconds;
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void InitializeSettings()
        {
            Settings s = new Settings();
            settings = s.LoadJson();

            milliseconds = settings["milliseconds"];
            tileSize = settings["tileSize"];
            formSize = settings["formSize"];
            halfFormSize = formSize / 2;
        }

        private void FormPaint(object sender, PaintEventArgs e)
        {
            apple.PaintApple(sender, e, tileSize);
            python.PaintPythonBody(sender, e, tileSize); 
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            python.Crawl(pressedKey, tileSize, formSize, apple);
            pressed = false;
            python.PythonCollision(timer);
            python.Wall�ollision(formSize, timer);
            this.Text = $"theRealOnePython apples: {apple.eaten}";
            Refresh();
        }

        private void Pause()
        {
            if (pause)
                timer.Start();
            else
                timer.Stop();
            pause = !pause;
        }

        private void KeyDownEvent (object sender, KeyEventArgs e)
        {
            if (!pressed)
            {
                switch (e.KeyCode)
                {
                    case Keys.W:
                        if (pressedKey != 's')
                            pressedKey = 'w';
                        break;
                    case Keys.A:
                        if (pressedKey != 'd')
                            pressedKey = 'a';
                        break;
                    case Keys.S:
                        if (pressedKey != 'w')
                            pressedKey = 's';
                        break;
                    case Keys.D:
                        if (pressedKey != 'a')
                            pressedKey = 'd';
                        break;
                }
                pressed = true;
            }
            if (e.KeyCode == Keys.Space)
                Pause();
        }
    }
}