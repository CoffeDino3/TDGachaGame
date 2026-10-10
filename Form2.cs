using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace TDGachaGame
{
    public partial class Form2 : Form
    {
        private const int FloaterCount = 26;
        private const int OutlineSamples = 120;
        private const float TickSeconds = 0.033f;
        private const float MorphHoldSeconds = 1.4f;
        private const float MorphTravelSeconds = 0.9f;
        private static readonly ShapeType[] AllShapes = (ShapeType[])Enum.GetValues(typeof(ShapeType));
        private static readonly PointF[][] Outlines = BuildOutlines();

        private static readonly Color[] Palette =
        {
            Color.FromArgb(225, 50, 50),
            Color.Cyan,
            Color.Gold,
            Color.MediumPurple,
            Color.LimeGreen,
            Color.RoyalBlue
        };

        private readonly Random rng = new Random(12345);
        private readonly float[] floaterX = new float[FloaterCount];
        private readonly float[] floaterY = new float[FloaterCount];
        private readonly float[] floaterSize = new float[FloaterCount];
        private readonly float[] floaterSpeed = new float[FloaterCount];
        private readonly float[] floaterAngle = new float[FloaterCount];
        private readonly float[] floaterSpin = new float[FloaterCount];
        private readonly ShapeType[] floaterKind = new ShapeType[FloaterCount];

        private float clock;

        public Form2()
        {
            InitializeComponent();
            UpdateMusicButton();

            for (int i = 0; i < FloaterCount; i++)
            {
                floaterX[i] = (float)rng.NextDouble();
                floaterY[i] = (float)rng.NextDouble();
                floaterSize[i] = rng.Next(14, 46);
                floaterSpeed[i] = 0.4f + (float)rng.NextDouble() * 0.9f;
                floaterAngle[i] = (float)(rng.NextDouble() * 360.0);
                floaterSpin[i] = ((float)rng.NextDouble() - 0.5f) * 1.2f;
                floaterKind[i] = AllShapes[i % AllShapes.Length];
            }
        }
        private void btnMusic_Click(object sender, EventArgs e)
        {
            MusicPlayer.ToggleMute();
            UpdateMusicButton();
        }

        private void UpdateMusicButton()
        {
            btnMusic.Text = MusicPlayer.Muted ? "\U0001F507" : "\U0001F50A";
        }

        private void tmrBanner_Tick(object sender, EventArgs e)
        {
            int height = Math.Max(1, pnlBanner.Height);

            for (int i = 0; i < FloaterCount; i++)
            {
                floaterY[i] -= floaterSpeed[i] / height;
                floaterAngle[i] += floaterSpin[i];

                if (floaterY[i] * height < -floaterSize[i])
                {
                    floaterY[i] = 1f + floaterSize[i] / height;
                    floaterX[i] = (float)rng.NextDouble();
                }
            }

            clock += TickSeconds;
            pnlBanner.Invalidate();
        }

        private void pnlBanner_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            int width = pnlBanner.Width;
            int height = pnlBanner.Height;

            for (int i = 0; i < FloaterCount; i++)
            {
                Color c = Palette[i % Palette.Length];
                DrawFloater(g, floaterKind[i], floaterX[i] * width, floaterY[i] * height, floaterSize[i], floaterAngle[i], c);
            }

            DrawMorphingLogo(g, width / 2f, height / 2f, Math.Min(width, height));
        }

        private static void DrawFloater(Graphics g, ShapeType type, float cx, float cy, float radius, float rotation, Color c)
        {
            PointF[] points = GetShapePoints(type, cx, cy, radius, rotation);

            if (type == ShapeType.Line)
            {
                using (var pen = new Pen(Color.FromArgb(170, c), 4f) { StartCap = LineCap.Round, EndCap = LineCap.Round })
                {
                    g.DrawLine(pen, points[0], points[1]);
                }
                return;
            }

            using (var fill = new SolidBrush(Color.FromArgb(28, c)))
            using (var pen = new Pen(Color.FromArgb(150, c), 2f))
            {
                g.FillPolygon(fill, points);
                g.DrawPolygon(pen, points);
            }
        }
        private void DrawMorphingLogo(Graphics g, float cx, float cy, float minSide)
        {
            float period = MorphHoldSeconds + MorphTravelSeconds;
            int step = (int)(clock / period);
            float phase = clock - step * period;

            float u = phase <= MorphHoldSeconds ? 0f : (phase - MorphHoldSeconds) / MorphTravelSeconds;
            u = u * u * (3f - 2f * u); // ease in/out

            float outerRadius = Math.Max(40f, Math.Min(110f, minSide * 0.32f));

            PointF[] outer = MorphOutline(step, u, outerRadius, clock * 10f, cx, cy);
            PointF[] inner = MorphOutline(step + 3, u, outerRadius * 0.62f, -clock * 22f, cx, cy);

            using (var outerPen = new Pen(Color.White, 5f) { LineJoin = LineJoin.Round })
            using (var innerPen = new Pen(Color.FromArgb(200, Color.Cyan), 3f) { LineJoin = LineJoin.Round })
            {
                g.DrawPolygon(outerPen, outer);
                g.DrawPolygon(innerPen, inner);
            }
        }

        private static PointF[] MorphOutline(int step, float u, float radius, float rotationDegrees, float cx, float cy)
        {
            PointF[] from = Outlines[step % AllShapes.Length];
            PointF[] to = Outlines[(step + 1) % AllShapes.Length];

            double rad = rotationDegrees * Math.PI / 180.0;
            float cos = (float)Math.Cos(rad);
            float sin = (float)Math.Sin(rad);

            var result = new PointF[OutlineSamples];
            for (int i = 0; i < OutlineSamples; i++)
            {
                float x = (from[i].X + (to[i].X - from[i].X) * u) * radius;
                float y = (from[i].Y + (to[i].Y - from[i].Y) * u) * radius;
                result[i] = new PointF(cx + x * cos - y * sin, cy + x * sin + y * cos);
            }

            return result;
        }

        private static PointF[][] BuildOutlines()
        {
            var all = new PointF[AllShapes.Length][];

            for (int s = 0; s < AllShapes.Length; s++)
            {
                PointF[] corners = GetShapePoints(AllShapes[s], 0f, 0f, 1f, 0f);
                int count = corners.Length;
                var samples = new PointF[OutlineSamples];
                for (int k = 0; k < OutlineSamples; k++)
                {
                    float t = k / (float)OutlineSamples * count;
                    int index = (int)t;
                    float local = t - index;
                    PointF a = corners[index % count];
                    PointF b = corners[(index + 1) % count];
                    samples[k] = new PointF(a.X + (b.X - a.X) * local, a.Y + (b.Y - a.Y) * local);
                }

                all[s] = samples;
            }

            return all;
        }
        private static PointF[] GetShapePoints(ShapeType type, float cx, float cy, float radius, float rotationDegrees)
        {
            int sides = GetSideCount(type);
            var points = new PointF[sides];

            if (type == ShapeType.Line)
            {
                double a = rotationDegrees * Math.PI / 180.0;
                float dx = (float)Math.Cos(a) * radius * 1.1f;
                float dy = (float)Math.Sin(a) * radius * 1.1f;
                points[0] = new PointF(cx - dx, cy - dy);
                points[1] = new PointF(cx + dx, cy + dy);
                return points;
            }

            for (int i = 0; i < sides; i++)
            {
                double angle = (rotationDegrees - 90.0 + i * 360.0 / sides) * Math.PI / 180.0;
                points[i] = new PointF(
                    cx + (float)(Math.Cos(angle) * radius),
                    cy + (float)(Math.Sin(angle) * radius));
            }

            return points;
        }

        private static int GetSideCount(ShapeType type)
        {
            switch (type)
            {
                case ShapeType.Triangle: return 3;
                case ShapeType.Square: return 4;
                case ShapeType.Pentagon: return 5;
                case ShapeType.Hexagon: return 6;
                case ShapeType.Heptagon: return 7;
                case ShapeType.Octagon: return 8;
                default: return 2; // Line
            }
        }
    }
}