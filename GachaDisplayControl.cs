using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace TDGachaGame
{
    public class GachaDisplayControl : Control
    {
        private enum Phase
        {
            Idle,
            Charging,
            Reveal,
            Done
        }

        private sealed class Particle
        {
            public int Kind;
            public float X, Y;
            public float VX, VY;
            public float Gravity;
            public float Drag;
            public float Life, MaxLife;
            public float Size;
            public float Rot, VRot;
            public float Angle, Radius;
            public Color Tint;
        }

        private sealed class Ring
        {
            public float Delay, Age, Life, MaxRadius, Rot, Spin;
            public bool Circle;
            public Color Tint;
        }

        private const int KindSpark = 0;
        private const int KindEmber = 1;
        private const int KindShard = 2;
        private const int KindBubble = 3;
        private const int KindStar = 4;
        private const int KindSpiral = 5;
        private const int KindWisp = 6;
        private const int KindGhost = 7;

        private static readonly Color[] RarityColors =
        {
            Color.FromArgb(200, 205, 220),
            Color.FromArgb(90, 210, 130),
            Color.FromArgb(90, 160, 255),
            Color.FromArgb(195, 115, 255),
            Color.FromArgb(255, 200, 60)
        };

        private readonly Random rng = new Random();
        private readonly Stopwatch clock = Stopwatch.StartNew();
        private readonly List<Particle> particles = new List<Particle>();
        private readonly List<Ring> rings = new List<Ring>();
        private readonly PointF[] ambientPos = new PointF[40];
        private readonly float[] ambientPhase = new float[40];

        private readonly SolidBrush brush = new SolidBrush(Color.White);
        private readonly Pen pen = new Pen(Color.White);
        private readonly Font bigFont = new Font("Segoe UI", 54f, FontStyle.Bold);

        private double lastClock;
        private double globalTime;

        private Phase phase = Phase.Idle;
        private PullResult result;
        private double t;
        private double duration = 1;
        private double chargeTime = 0.5;

        private float flash;
        private float shake;
        private float slashAge = -1f;
        private float trickleAcc;
        private int silhouetteIndex;
        private double silhouetteTimer;
        public event EventHandler Revealed;
        public event EventHandler Finished;
        public static Color GetRarityColor(int rarityIndex)
        {
            return RarityColors[Math.Max(0, Math.Min(RarityColors.Length - 1, rarityIndex))];
        }

        public GachaDisplayControl()
        {
            SetStyle(
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.UserPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.ResizeRedraw,
                true);

            BackColor = Color.FromArgb(10, 12, 20);

            pen.StartCap = LineCap.Round;
            pen.EndCap = LineCap.Round;
            pen.LineJoin = LineJoin.Round;

            for (int i = 0; i < ambientPos.Length; i++)
            {
                ambientPos[i] = new PointF((float)rng.NextDouble(), (float)rng.NextDouble());
                ambientPhase[i] = (float)(rng.NextDouble() * 6.28);
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsPlaying => phase == Phase.Charging || phase == Phase.Reveal;
        public void Play(PullResult pull, bool fast)
        {
            result = pull;
            phase = Phase.Charging;
            t = 0;
            duration = fast ? 0.9 : 2.3;
            chargeTime = fast ? 0.32 : 1.0;
            flash = 0;
            shake = 0;
            slashAge = -1f;
            trickleAcc = 0;
            particles.Clear();
            rings.Clear();
        }
        public void ShowResult(PullResult pull)
        {
            result = pull;
            phase = Phase.Done;
            t = duration = 1;
            flash = 0;
            shake = 0;
            slashAge = -1f;
            trickleAcc = 0;
            particles.Clear();
            rings.Clear();
            Invalidate();
        }
        public void ClearDisplay()
        {
            result = null;
            phase = Phase.Idle;
            particles.Clear();
            rings.Clear();
            flash = 0;
            shake = 0;
            slashAge = -1f;
            Invalidate();
        }
        public void Step()
        {
            double now = clock.Elapsed.TotalSeconds;
            float dt = (float)Math.Min(0.05, now - lastClock);
            lastClock = now;
            globalTime += dt;

            UpdateParticles(dt);
            UpdateRings(dt);

            if (flash > 0f)
            {
                flash = Math.Max(0f, flash - dt * 2.2f);
            }

            if (shake > 0f)
            {
                shake = Math.Max(0f, shake - dt * 22f);
            }

            if (slashAge >= 0f)
            {
                slashAge += dt;
                if (slashAge > 0.45f)
                {
                    slashAge = -1f;
                }
            }

            bool justFinished = false;

            if (phase == Phase.Charging)
            {
                t += dt;
                UpdateCharging(dt);

                if (t >= chargeTime)
                {
                    FireBurst();
                    phase = Phase.Reveal;
                    Revealed?.Invoke(this, EventArgs.Empty);
                }
            }
            else if (phase == Phase.Reveal)
            {
                t += dt;
                TrickleParticles(dt);

                if (t >= duration)
                {
                    phase = Phase.Done;
                    justFinished = true;
                }
            }
            else if (phase == Phase.Done)
            {
                TrickleParticles(dt * 0.5f);
            }

            Invalidate();

            if (justFinished)
            {
                Finished?.Invoke(this, EventArgs.Empty);
            }
        }

        private void UpdateCharging(float dt)
        {
            float p = Clamp01((float)(t / chargeTime));

            trickleAcc += dt * (40f + 140f * p);
            while (trickleAcc >= 1f)
            {
                trickleAcc -= 1f;
                float a = Rand(0f, (float)(Math.PI * 2));
                float dist = Math.Min(Width, Height) * 0.55f + Rand(0f, 60f);
                float life = Rand(0.3f, 0.5f);

                Color tint = Lerp(Color.White, RarityColors[result.RarityIndex], 0.6f);
                var s = new Particle
                {
                    Kind = KindSpark,
                    X = (float)Math.Cos(a) * dist,
                    Y = (float)Math.Sin(a) * dist,
                    Life = life,
                    MaxLife = life,
                    Size = Rand(2f, 4f),
                    Tint = tint
                };
                s.VX = -s.X / life;
                s.VY = -s.Y / life;
                particles.Add(s);
            }

            silhouetteTimer += dt;
            if (silhouetteTimer > 0.07)
            {
                silhouetteTimer = 0;
                silhouetteIndex++;
            }
        }

        private void FireBurst()
        {
            UnitId unit = result.Unit;
            float k = 1f + result.RarityIndex * 0.35f;
            Color glow = GlowColor(unit.Color);

            flash = unit.Color == ShapeColor.White ? 1f : Math.Min(1f, 0.55f + 0.12f * result.RarityIndex);
            shake = ShakeFor(unit.Type) * k;
            trickleAcc = 0;
            int ringCount = 3 + (result.RarityIndex >= 3 ? 1 : 0) + (result.RarityIndex >= 4 ? 1 : 0);
            float maxR = Math.Min(Width, Height) * 0.7f;

            for (int i = 0; i < ringCount; i++)
            {
                rings.Add(new Ring
                {
                    Delay = i * 0.09f,
                    Life = 0.9f,
                    MaxRadius = maxR * (0.6f + 0.12f * i),
                    Rot = Rand(0f, 360f),
                    Spin = SpinFor(unit.Type) * (i % 2 == 0 ? 1f : -1f),
                    Tint = glow
                });
            }
            if (unit.Color == ShapeColor.Blue)
            {
                for (int i = 0; i < 3; i++)
                {
                    rings.Add(new Ring
                    {
                        Delay = 0.05f + i * 0.14f,
                        Life = 1.1f,
                        MaxRadius = maxR * (0.5f + 0.2f * i),
                        Circle = true,
                        Tint = Color.DeepSkyBlue
                    });
                }
            }

            if (unit.Type == ShapeType.Line)
            {
                slashAge = 0f;
            }

            SpawnBurst(unit.Color, (int)(26 * k));
        }

        private void SpawnBurst(ShapeColor color, int count)
        {
            float R = StageRadius();

            for (int i = 0; i < count; i++)
            {
                float a = Rand(0f, (float)(Math.PI * 2));
                Particle p;

                switch (color)
                {
                    case ShapeColor.Red:
                        p = Make(KindEmber, a, Rand(140f, 380f), Rand(0.8f, 1.5f), Rand(3f, 7f),
                            Pick(Color.OrangeRed, Color.Orange, Color.Gold, Color.FromArgb(225, 50, 50)));
                        p.Gravity = -70f;
                        p.Drag = 0.9f;
                        break;

                    case ShapeColor.Blue:
                        p = Make(KindBubble, a, Rand(80f, 260f), Rand(0.9f, 1.6f), Rand(4f, 10f),
                            Pick(Color.RoyalBlue, Color.DeepSkyBlue, Color.CornflowerBlue));
                        p.Gravity = -20f;
                        p.Drag = 1.2f;
                        break;

                    case ShapeColor.Cyan:
                        p = Make(KindShard, a, Rand(160f, 440f), Rand(0.7f, 1.3f), Rand(7f, 15f),
                            Pick(Color.Cyan, Color.White, Color.LightCyan, Color.DeepSkyBlue));
                        p.Drag = 1.1f;
                        p.VRot = Rand(-420f, 420f);
                        break;

                    case ShapeColor.Green:
                        p = Make(KindBubble, a, Rand(50f, 190f), Rand(1.1f, 2.0f), Rand(4f, 11f),
                            Pick(Color.LimeGreen, Color.Lime, Color.YellowGreen));
                        p.Gravity = -45f;
                        p.Drag = 1.0f;
                        break;

                    case ShapeColor.Purple:
                        p = Make(KindSpiral, a, 0f, Rand(1.0f, 1.7f), Rand(3f, 6f),
                            Pick(Color.MediumPurple, Color.Violet, Color.Magenta, Color.BlueViolet));
                        p.Radius = 8f;
                        p.VY = Rand(120f, 300f);
                        p.VRot = Rand(3f, 6.5f) * (rng.Next(2) == 0 ? 1f : -1f);
                        PlaceSpiral(p);
                        break;

                    case ShapeColor.Yellow:
                        p = Make(KindStar, a, Rand(150f, 420f), Rand(0.8f, 1.4f), Rand(6f, 14f),
                            Pick(Color.Gold, Color.Yellow, Color.White, Color.Orange));
                        p.Drag = 1.0f;
                        p.VRot = Rand(-300f, 300f);
                        break;

                    case ShapeColor.Black:
                        {
                            float dist = Rand(150f, 300f);
                            p = Make(KindSpark, a, 0f, Rand(0.5f, 0.9f), Rand(3f, 6f),
                                Pick(Color.FromArgb(150, 100, 220), Color.FromArgb(90, 60, 150), Color.DimGray));
                            p.X = (float)Math.Cos(a) * dist;
                            p.Y = (float)Math.Sin(a) * dist;
                            p.VX = -p.X / p.Life * 1.05f;
                            p.VY = -p.Y / p.Life * 1.05f;
                            break;
                        }

                    default:
                        p = Make(KindWisp, a, Rand(40f, 160f), Rand(1.2f, 2.2f), Rand(10f, 24f),
                            Pick(Color.White, Color.Gainsboro, Color.LightYellow));
                        p.Gravity = -25f;
                        p.Drag = 0.8f;
                        break;
                }

                particles.Add(p);
            }
            if (color == ShapeColor.Black)
            {
                for (int i = 0; i < 5; i++)
                {
                    particles.Add(MakeGhost(R));
                }
            }
        }
        private void TrickleParticles(float dt)
        {
            if (result == null)
            {
                return;
            }

            trickleAcc += dt * (22f + 6f * result.RarityIndex);
            while (trickleAcc >= 1f)
            {
                trickleAcc -= 1f;
                SpawnTrickleOne(result.Unit.Color);
            }
        }

        private void SpawnTrickleOne(ShapeColor color)
        {
            float R = StageRadius();
            float up = -(float)Math.PI / 2f;
            float x = Rand(-R, R);
            float y = Rand(-0.3f * R, 0.9f * R);
            Particle p;

            switch (color)
            {
                case ShapeColor.Red:
                    p = Make(KindEmber, up + Rand(-0.4f, 0.4f), Rand(50f, 130f), Rand(0.9f, 1.5f), Rand(2f, 5f),
                        Pick(Color.OrangeRed, Color.Orange, Color.Gold));
                    break;

                case ShapeColor.Blue:
                    p = Make(KindBubble, up + Rand(-0.3f, 0.3f), Rand(30f, 70f), Rand(1.0f, 1.8f), Rand(3f, 7f),
                        Pick(Color.RoyalBlue, Color.DeepSkyBlue));
                    break;

                case ShapeColor.Cyan:
                    p = Make(KindShard, Rand(0f, (float)(Math.PI * 2)), Rand(30f, 80f), Rand(0.8f, 1.2f), Rand(5f, 9f),
                        Pick(Color.Cyan, Color.White, Color.LightCyan));
                    p.VRot = Rand(-200f, 200f);
                    break;

                case ShapeColor.Green:
                    p = Make(KindBubble, up + Rand(-0.3f, 0.3f), Rand(30f, 70f), Rand(1.2f, 2.0f), Rand(3f, 8f),
                        Pick(Color.LimeGreen, Color.Lime, Color.YellowGreen));
                    p.Gravity = -10f;
                    break;

                case ShapeColor.Purple:
                    p = Make(KindSpiral, Rand(0f, (float)(Math.PI * 2)), 0f, Rand(1.0f, 1.6f), Rand(2f, 4f),
                        Pick(Color.MediumPurple, Color.Violet, Color.BlueViolet));
                    p.Radius = Rand(R * 0.9f, R * 1.3f);
                    p.VY = Rand(-25f, 25f);
                    p.VRot = Rand(1.5f, 2.6f) * (rng.Next(2) == 0 ? 1f : -1f);
                    PlaceSpiral(p);
                    break;

                case ShapeColor.Yellow:
                    p = Make(KindStar, 0f, 0f, Rand(0.4f, 0.8f), Rand(4f, 9f), Pick(Color.Gold, Color.White, Color.Yellow));
                    p.VRot = 200f;
                    break;

                case ShapeColor.Black:
                    if (rng.Next(4) == 0)
                    {
                        p = MakeGhost(R);
                    }
                    else
                    {
                        p = Make(KindSpark, up, Rand(30f, 80f), Rand(0.8f, 1.4f), Rand(2f, 4f),
                            Pick(Color.FromArgb(150, 100, 220), Color.FromArgb(90, 60, 150)));
                    }
                    break;

                default:
                    p = Make(KindWisp, up + Rand(-0.5f, 0.5f), Rand(20f, 50f), Rand(1.5f, 2.2f), Rand(8f, 16f),
                        Pick(Color.White, Color.Gainsboro));
                    break;
            }

            if (p.Kind != KindSpiral && p.Kind != KindGhost)
            {
                p.X = x;
                p.Y = y;
            }

            particles.Add(p);
        }

        private Particle MakeGhost(float R)
        {
            var ghost = Make(KindGhost, -(float)Math.PI / 2f, Rand(40f, 90f), Rand(1.6f, 2.4f), R * Rand(0.15f, 0.3f),
                Pick(Color.FromArgb(150, 100, 220), Color.FromArgb(200, 190, 255)));
            ghost.X = Rand(-R, R);
            ghost.Y = Rand(0f, R);
            return ghost;
        }

        private Particle Make(int kind, float angle, float speed, float life, float size, Color tint)
        {
            return new Particle
            {
                Kind = kind,
                Angle = angle,
                VX = (float)Math.Cos(angle) * speed,
                VY = (float)Math.Sin(angle) * speed,
                Life = life,
                MaxLife = life,
                Size = size,
                Tint = tint,
                Rot = Rand(0f, 360f)
            };
        }

        private static void PlaceSpiral(Particle p)
        {
            p.X = (float)Math.Cos(p.Angle) * p.Radius;
            p.Y = (float)Math.Sin(p.Angle) * p.Radius;
        }

        private void UpdateParticles(float dt)
        {
            for (int i = particles.Count - 1; i >= 0; i--)
            {
                Particle p = particles[i];
                p.Life -= dt;

                if (p.Life <= 0f)
                {
                    particles.RemoveAt(i);
                    continue;
                }

                if (p.Kind == KindSpiral)
                {
                    p.Angle += p.VRot * dt;
                    p.Radius += p.VY * dt;
                    PlaceSpiral(p);
                    continue;
                }

                float keep = Math.Max(0f, 1f - p.Drag * dt);
                p.VX *= keep;
                p.VY = p.VY * keep + p.Gravity * dt;
                p.X += p.VX * dt;
                p.Y += p.VY * dt;
                p.Rot += p.VRot * dt;

                if (p.Kind == KindBubble)
                {
                    p.X += (float)Math.Sin(p.Life * 7f + p.Rot) * 18f * dt;
                }
            }
        }

        private void UpdateRings(float dt)
        {
            for (int i = rings.Count - 1; i >= 0; i--)
            {
                Ring r = rings[i];

                if (r.Delay > 0f)
                {
                    r.Delay -= dt;
                    continue;
                }

                r.Age += dt;
                if (r.Age >= r.Life)
                {
                    rings.RemoveAt(i);
                }
            }
        }

        private static float ShakeFor(ShapeType type)
        {
            switch (type)
            {
                case ShapeType.Triangle: return 1f;
                case ShapeType.Square: return 2.5f;
                case ShapeType.Pentagon: return 4f;
                case ShapeType.Hexagon: return 7f;
                case ShapeType.Heptagon: return 9f;
                case ShapeType.Octagon: return 11f;
                default: return 0f;
            }
        }

        private static float SpinFor(ShapeType type)
        {
            switch (type)
            {
                case ShapeType.Line: return 0f;
                case ShapeType.Triangle: return 140f;
                case ShapeType.Square: return 70f;
                case ShapeType.Pentagon: return 45f;
                default: return 30f;
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.Clear(BackColor);

            DrawAmbient(g);

            float cx = Width / 2f;
            float cy = Height * 0.50f;
            float R = StageRadius();

            var saved = g.Save();
            if (shake > 0.05f)
            {
                g.TranslateTransform(Rand(-shake, shake), Rand(-shake, shake));
            }

            switch (phase)
            {
                case Phase.Idle:
                    DrawIdle(g, cx, cy, R);
                    break;

                case Phase.Charging:
                    DrawCharging(g, cx, cy, R);
                    break;

                default:
                    DrawRevealed(g, cx, cy, R);
                    break;
            }

            g.Restore(saved);

            if (flash > 0.01f)
            {
                brush.Color = Fade(Color.White, flash * 0.85f);
                g.FillRectangle(brush, 0, 0, Width, Height);
            }
        }

        private void DrawAmbient(Graphics g)
        {
            for (int i = 0; i < ambientPos.Length; i++)
            {
                float speed = 0.01f + (ambientPhase[i] % 1f) * 0.02f;
                double v = ambientPos[i].Y - globalTime * speed;
                float y = (float)(v - Math.Floor(v)) * Height;
                float x = ambientPos[i].X * Width + (float)Math.Sin(globalTime * 0.6 + ambientPhase[i]) * 8f;
                float alpha = 0.12f + 0.12f * (float)Math.Sin(globalTime * 1.5 + ambientPhase[i]);
                float s = 1.2f + (ambientPhase[i] % 1.5f);

                brush.Color = Fade(Color.FromArgb(150, 170, 230), alpha);
                g.FillEllipse(brush, x - s, y - s, s * 2f, s * 2f);
            }
        }

        private void DrawIdle(Graphics g, float cx, float cy, float R)
        {
            float pulse = 0.5f + 0.5f * (float)Math.Sin(globalTime * 2.0);
            float r = R * 0.8f * (1f + 0.04f * pulse);

            DrawGlow(g, cx, cy, Color.FromArgb(90, 110, 190), 0.12f + 0.1f * pulse);

            pen.Color = Fade(Color.FromArgb(120, 135, 175), 0.45f + 0.3f * pulse);
            pen.Width = 3f;
            g.DrawEllipse(pen, cx - r, cy - r, r * 2f, r * 2f);

            pen.Color = Fade(Color.FromArgb(120, 135, 175), 0.2f);
            pen.Width = 2f;
            g.DrawEllipse(pen, cx - r * 1.25f, cy - r * 1.25f, r * 2.5f, r * 2.5f);

            DrawCenteredText(g, "?", bigFont, Color.FromArgb(120, 135, 175), 0.55f + 0.25f * pulse, cy, 90f);
        }

        private void DrawCharging(Graphics g, float cx, float cy, float R)
        {
            float p = Clamp01((float)(t / chargeTime));
            Color rarityColor = RarityColors[result.RarityIndex];

            DrawGlow(g, cx, cy, rarityColor, 0.15f + 0.4f * p);
            DrawParticles(g, cx, cy);
            float tremble = 3f * p;
            float ox = Rand(-tremble, tremble);
            float oy = Rand(-tremble, tremble);
            float orbR = R * (0.45f + 0.5f * p);

            brush.Color = Fade(rarityColor, 0.18f + 0.2f * p);
            g.FillEllipse(brush, cx + ox - orbR, cy + oy - orbR, orbR * 2f, orbR * 2f);
            pen.Color = Fade(Color.White, 0.8f);
            pen.Width = 3f;
            g.DrawEllipse(pen, cx + ox - orbR, cy + oy - orbR, orbR * 2f, orbR * 2f);
            ShapeType[] shapes = PlayerProfile.PlayableShapes;
            ShapeType flickerShape = shapes[silhouetteIndex % shapes.Length];
            Color flickerColor = GlowColor((ShapeColor)(silhouetteIndex % 8));

            DrawShape(g, flickerShape, Color.Empty, Fade(flickerColor, 0.75f), cx + ox, cy + oy,
                orbR * 0.9f, (float)globalTime * 180f, 3f, false);
        }

        private void DrawRevealed(Graphics g, float cx, float cy, float R)
        {
            UnitId unit = result.Unit;
            Color glow = GlowColor(unit.Color);
            float u = phase == Phase.Reveal
                ? Clamp01((float)((t - chargeTime) / (duration - chargeTime)))
                : 1f;

            DrawGlow(g, cx, cy, glow, phase == Phase.Reveal ? 0.55f : 0.4f);

            if (unit.Color == ShapeColor.Yellow || unit.Color == ShapeColor.White)
            {
                DrawRays(g, cx, cy, glow, phase == Phase.Reveal ? 0.2f : 0.1f);
            }

            DrawRings(g, cx, cy);
            DrawParticles(g, cx, cy);
            DrawUnit(g, cx, cy, R, u);
            DrawSlash(g, cx, cy);
        }

        private void DrawUnit(Graphics g, float cx, float cy, float R, float u)
        {
            UnitId unit = result.Unit;
            Color fill = UnitSlotControl.ToDrawingColor(unit.Color);
            Color glow = GlowColor(unit.Color);

            float scale = EaseOutBack(Clamp01(u / 0.35f));
            float r = R * scale;
            float bob = (float)Math.Sin(globalTime * 2.2) * 3f;
            float y = cy + bob;

            if (unit.Color == ShapeColor.Cyan && u < 1f)
            {
                float spread = R * 1.4f * EaseOutCubic(Clamp01(u / 0.5f));
                float ghostAlpha = (1f - u) * 0.6f;

                for (int side = -1; side <= 1; side += 2)
                {
                    DrawShape(g, unit.Type, Fade(fill, ghostAlpha), Fade(Color.White, ghostAlpha), cx + side * spread, y,
                        r * 0.75f, 0f, 2f, true);
                }
            }
            DrawShape(g, unit.Type, Fade(glow, 0.22f), Color.Empty, cx, y, r * 1.3f, 0f, 0f, true);
            float satAlpha = Clamp01((u - 0.2f) / 0.4f) * 0.9f;
            if (satAlpha > 0.01f)
            {
                int count = UnitSlotControl.GetSideCount(unit.Type);
                float orbit = r * 1.55f;
                float satR = Math.Max(6f, r * 0.12f);

                for (int i = 0; i < count; i++)
                {
                    double a = (globalTime * 130.0 + i * 360.0 / count) * Math.PI / 180.0;
                    float sx = cx + (float)Math.Cos(a) * orbit;
                    float sy = y + (float)Math.Sin(a) * orbit;

                    DrawShape(g, unit.Type, Fade(glow, satAlpha), Fade(Color.White, satAlpha * 0.8f), sx, sy, satR,
                        (float)(a * 180.0 / Math.PI), 1.5f, true);
                }
            }
            DrawShape(g, unit.Type, fill, Color.White, cx, y, r, 0f, Math.Max(2f, R * 0.035f), true);
        }

        private void DrawGlow(Graphics g, float cx, float cy, Color tint, float strength)
        {
            float rad = Math.Min(Width, Height) * 0.75f;

            using (var path = new GraphicsPath())
            {
                path.AddEllipse(cx - rad, cy - rad, rad * 2f, rad * 2f);

                using (var glowBrush = new PathGradientBrush(path))
                {
                    glowBrush.CenterColor = Fade(tint, strength);
                    glowBrush.SurroundColors = new[] { Color.FromArgb(0, tint.R, tint.G, tint.B) };
                    g.FillPath(glowBrush, path);
                }
            }
        }

        private void DrawRays(Graphics g, float cx, float cy, Color tint, float alpha)
        {
            float length = Math.Max(Width, Height);
            brush.Color = Fade(tint, alpha);

            for (int i = 0; i < 12; i++)
            {
                double a = (i * 30.0 + globalTime * 20.0) * Math.PI / 180.0;
                double half = 4.0 * Math.PI / 180.0;

                g.FillPolygon(brush, new[]
                {
                    new PointF(cx, cy),
                    new PointF(cx + (float)Math.Cos(a - half) * length, cy + (float)Math.Sin(a - half) * length),
                    new PointF(cx + (float)Math.Cos(a + half) * length, cy + (float)Math.Sin(a + half) * length)
                });
            }
        }

        private void DrawRings(Graphics g, float cx, float cy)
        {
            ShapeType type = result.Unit.Type;

            foreach (Ring r in rings)
            {
                if (r.Delay > 0f)
                {
                    continue;
                }

                float q = Clamp01(r.Age / r.Life);
                float radius = r.MaxRadius * EaseOutCubic(q);
                float alpha = (1f - q) * 0.8f;

                pen.Color = Fade(r.Tint, alpha);
                pen.Width = 2f + (1f - q) * 5f;

                if (r.Circle)
                {
                    g.DrawEllipse(pen, cx - radius, cy - radius, radius * 2f, radius * 2f);
                }
                else if (type == ShapeType.Line)
                {
                    float gap = radius * 0.6f;
                    g.DrawLine(pen, cx - radius, cy - gap, cx + radius, cy - gap);
                    g.DrawLine(pen, cx - radius, cy + gap, cx + radius, cy + gap);
                }
                else
                {
                    DrawShape(g, type, Color.Empty, Fade(r.Tint, alpha), cx, cy, radius, r.Rot + r.Spin * r.Age,
                        2f + (1f - q) * 5f, false);
                }
            }
        }

        private void DrawSlash(Graphics g, float cx, float cy)
        {
            if (slashAge < 0f)
            {
                return;
            }

            float len = Math.Min(Width, Height) * 0.9f;

            for (int i = 0; i < 2; i++)
            {
                float q = Clamp01((slashAge - i * 0.12f) / 0.33f);
                if (q <= 0f || q >= 1f)
                {
                    continue;
                }

                float dir = i == 0 ? 1f : -1f;
                float head = EaseOutCubic(Clamp01(q * 2.5f));
                var from = new PointF(cx - len, cy + dir * len * 0.55f);
                var to = new PointF(cx + len, cy - dir * len * 0.55f);
                var tip = new PointF(from.X + (to.X - from.X) * head, from.Y + (to.Y - from.Y) * head);

                pen.Color = Fade(Color.White, 1f - q);
                pen.Width = 3f + 9f * (1f - q);
                g.DrawLine(pen, from, tip);
            }
        }

        private void DrawParticles(Graphics g, float cx, float cy)
        {
            foreach (Particle p in particles)
            {
                float a = Clamp01(p.Life / p.MaxLife);
                float px = cx + p.X;
                float py = cy + p.Y;
                float s;

                switch (p.Kind)
                {
                    case KindSpark:
                    case KindEmber:
                        s = p.Kind == KindEmber ? p.Size * (0.4f + 0.6f * a) : p.Size;
                        brush.Color = Fade(p.Tint, a * 0.25f);
                        g.FillEllipse(brush, px - s * 2f, py - s * 2f, s * 4f, s * 4f);
                        brush.Color = Fade(p.Tint, a);
                        g.FillEllipse(brush, px - s, py - s, s * 2f, s * 2f);
                        break;

                    case KindShard:
                        {
                            double ang = p.Rot * Math.PI / 180.0;
                            s = p.Size;
                            brush.Color = Fade(p.Tint, a);
                            g.FillPolygon(brush, new[]
                            {
                            new PointF(px + (float)Math.Cos(ang) * s * 1.5f, py + (float)Math.Sin(ang) * s * 1.5f),
                            new PointF(px + (float)Math.Cos(ang + 2.4) * s * 0.7f, py + (float)Math.Sin(ang + 2.4) * s * 0.7f),
                            new PointF(px + (float)Math.Cos(ang - 2.4) * s * 0.7f, py + (float)Math.Sin(ang - 2.4) * s * 0.7f)
                        });
                            break;
                        }

                    case KindBubble:
                        s = p.Size;
                        pen.Color = Fade(p.Tint, a * 0.9f);
                        pen.Width = 1.6f;
                        g.DrawEllipse(pen, px - s, py - s, s * 2f, s * 2f);
                        brush.Color = Fade(Color.White, a * 0.6f);
                        g.FillEllipse(brush, px - s * 0.5f, py - s * 0.5f, s * 0.5f, s * 0.5f);
                        break;

                    case KindStar:
                        DrawStar(g, px, py, p.Size * (0.5f + 0.5f * a), p.Rot, Fade(p.Tint, a));
                        break;

                    case KindSpiral:
                        s = p.Size;
                        brush.Color = Fade(p.Tint, a * 0.3f);
                        g.FillEllipse(brush, px - s * 2f, py - s * 2f, s * 4f, s * 4f);
                        brush.Color = Fade(p.Tint, a);
                        g.FillEllipse(brush, px - s, py - s, s * 2f, s * 2f);
                        break;

                    case KindWisp:
                        for (int k = 3; k >= 1; k--)
                        {
                            float rs = p.Size * k / 3f * 1.6f;
                            brush.Color = Fade(p.Tint, a * 0.16f);
                            g.FillEllipse(brush, px - rs, py - rs, rs * 2f, rs * 2f);
                        }
                        break;

                    case KindGhost:
                        if (result != null)
                        {
                            DrawShape(g, result.Unit.Type, Color.Empty, Fade(p.Tint, a * 0.6f), px, py, p.Size, 0f, 2f, false);
                        }
                        break;
                }
            }
        }

        private void DrawStar(Graphics g, float px, float py, float size, float rotDeg, Color color)
        {
            var pts = new PointF[8];

            for (int i = 0; i < 8; i++)
            {
                double ang = (rotDeg + i * 45.0) * Math.PI / 180.0;
                float rad = i % 2 == 0 ? size : size * 0.28f;
                pts[i] = new PointF(px + (float)Math.Cos(ang) * rad, py + (float)Math.Sin(ang) * rad);
            }

            brush.Color = color;
            g.FillPolygon(brush, pts);
        }
        private void DrawShape(Graphics g, ShapeType type, Color fill, Color outline, float cx, float cy, float radius,
            float rotDeg, float outlineWidth, bool filled)
        {
            if (radius < 1f)
            {
                return;
            }

            bool hasOutline = outline != Color.Empty && outlineWidth > 0f;
            bool hasFill = filled && fill != Color.Empty;

            if (type == ShapeType.Line)
            {
                double ang = rotDeg * Math.PI / 180.0;
                var a = new PointF(cx - (float)Math.Cos(ang) * radius, cy - (float)Math.Sin(ang) * radius);
                var b = new PointF(cx + (float)Math.Cos(ang) * radius, cy + (float)Math.Sin(ang) * radius);
                float thick = Math.Max(4f, radius * 0.14f);

                if (hasFill)
                {
                    if (hasOutline)
                    {
                        pen.Color = outline;
                        pen.Width = thick + outlineWidth * 2f;
                        g.DrawLine(pen, a, b);
                    }

                    pen.Color = fill;
                    pen.Width = thick;
                    g.DrawLine(pen, a, b);
                }
                else if (hasOutline)
                {
                    pen.Color = outline;
                    pen.Width = outlineWidth;
                    g.DrawLine(pen, a, b);
                }

                return;
            }

            PointF[] pts = Polygon(UnitSlotControl.GetSideCount(type), cx, cy, radius, rotDeg);

            if (hasFill)
            {
                brush.Color = fill;
                g.FillPolygon(brush, pts);
            }

            if (hasOutline)
            {
                pen.Color = outline;
                pen.Width = outlineWidth;
                g.DrawPolygon(pen, pts);
            }
        }
        private static PointF[] Polygon(int sides, float cx, float cy, float radius, float rotDeg)
        {
            double start = (sides % 2 == 1 ? -90.0 : -90.0 + 180.0 / sides) + rotDeg;
            var pts = new PointF[sides];

            for (int i = 0; i < sides; i++)
            {
                double angle = (start + i * 360.0 / sides) * Math.PI / 180.0;
                pts[i] = new PointF(cx + (float)(Math.Cos(angle) * radius), cy + (float)(Math.Sin(angle) * radius));
            }

            return pts;
        }

        private void DrawCenteredText(Graphics g, string text, Font font, Color color, float alpha, float centerY, float height)
        {
            if (alpha <= 0.01f)
            {
                return;
            }

            brush.Color = Fade(color, alpha);

            using (var format = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
            {
                g.DrawString(text, font, brush, new RectangleF(0f, centerY - height / 2f, Width, height), format);
            }
        }
        private float StageRadius()
        {
            return Math.Max(40f, Math.Min(Math.Min(Width, Height) * 0.28f, 120f));
        }
        private static Color GlowColor(ShapeColor color)
        {
            if (color == ShapeColor.Black)
            {
                return Color.FromArgb(150, 100, 220);
            }

            return UnitSlotControl.ToDrawingColor(color);
        }

        private float Rand(float min, float max)
        {
            return min + (float)rng.NextDouble() * (max - min);
        }

        private Color Pick(params Color[] colors)
        {
            return colors[rng.Next(colors.Length)];
        }

        private static float Clamp01(float v)
        {
            return v < 0f ? 0f : (v > 1f ? 1f : v);
        }

        private static Color Fade(Color c, float alpha)
        {
            int a = (int)(alpha * 255f);
            return Color.FromArgb(a < 0 ? 0 : (a > 255 ? 255 : a), c.R, c.G, c.B);
        }

        private static Color Lerp(Color a, Color b, float t)
        {
            return Color.FromArgb(
                (int)(a.R + (b.R - a.R) * t),
                (int)(a.G + (b.G - a.G) * t),
                (int)(a.B + (b.B - a.B) * t));
        }

        private static float EaseOutCubic(float x)
        {
            float inv = 1f - x;
            return 1f - inv * inv * inv;
        }
        private static float EaseOutBack(float x)
        {
            const float c1 = 1.70158f;
            const float c3 = c1 + 1f;
            float m = x - 1f;
            return 1f + c3 * m * m * m + c1 * m * m;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                brush.Dispose();
                pen.Dispose();
                bigFont.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}