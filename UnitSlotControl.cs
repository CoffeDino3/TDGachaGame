using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace TDGachaGame
{
    public class UnitSlotControl : Control
    {
        private int slotNumber = 1;
        private UnitId? unit;
        private bool showSlotNumber = true;
        private bool selected;
        private bool interactive;
        private bool hovering;
        private string emptyText = "Empty";
        private string badge = "";
        private int level;
        private Color selectionColor = Color.Gold;

        public UnitSlotControl()
        {
            SetStyle(
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.UserPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.ResizeRedraw,
                true);

            BackColor = Color.FromArgb(24, 28, 40);
        }

        [Category("Game")]
        [DefaultValue(1)]
        public int SlotNumber
        {
            get => slotNumber;
            set
            {
                slotNumber = value;
                Invalidate();
            }
        }

        [Category("Game")]
        [DefaultValue(true)]
        public bool ShowSlotNumber
        {
            get => showSlotNumber;
            set
            {
                showSlotNumber = value;
                Invalidate();
            }
        }
        [Category("Game")]
        [DefaultValue("Empty")]
        public string EmptyText
        {
            get => emptyText;
            set
            {
                emptyText = value ?? "";
                Invalidate();
            }
        }
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool Selected
        {
            get => selected;
            set
            {
                selected = value;
                Invalidate();
            }
        }
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string Badge
        {
            get => badge;
            set
            {
                badge = value ?? "";
                Invalidate();
            }
        }
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool Interactive
        {
            get => interactive;
            set
            {
                interactive = value;
                Cursor = value ? Cursors.Hand : Cursors.Default;
                Invalidate();
            }
        }
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int Level
        {
            get => level;
            set
            {
                level = value;
                Invalidate();
            }
        }
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Color SelectionColor
        {
            get => selectionColor;
            set
            {
                selectionColor = value;
                Invalidate();
            }
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            hovering = true;
            Invalidate();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            hovering = false;
            Invalidate();
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public UnitId? Unit
        {
            get => unit;
            set
            {
                unit = value;
                Invalidate();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.Clear(BackColor);
            g.SmoothingMode = SmoothingMode.AntiAlias;

            Color borderColor = selected
                ? selectionColor
                : (interactive && hovering ? Color.FromArgb(130, 145, 185) : Color.FromArgb(70, 80, 105));

            using (var borderPen = new Pen(borderColor, selected ? 2.5f : 1.5f))
            {
                g.DrawRectangle(borderPen, 1, 1, Width - 3, Height - 3);
            }

            if (showSlotNumber)
            {
                using (var numberFont = new Font("Segoe UI", 8.5f, FontStyle.Bold))
                using (var numberBrush = new SolidBrush(Color.Silver))
                {
                    g.DrawString(slotNumber.ToString(), numberFont, numberBrush, 6, 4);
                }
            }

            if (unit != null && level > 0)
            {
                using (var levelFont = new Font("Segoe UI", 8.5f, FontStyle.Bold))
                using (var levelBrush = new SolidBrush(Color.Gold))
                {
                    g.DrawString("Lv " + level, levelFont, levelBrush, showSlotNumber ? 30 : 6, 4);
                }
            }

            if (unit != null && badge.Length > 0)
            {
                using (var badgeFont = new Font("Segoe UI", 7.5f, FontStyle.Bold))
                using (var badgeBack = new SolidBrush(Color.FromArgb(46, 160, 90)))
                using (var badgeText = new SolidBrush(Color.White))
                {
                    SizeF textSize = g.MeasureString(badge, badgeFont);
                    var badgeRect = new RectangleF(Width - textSize.Width - 10f, 5f, textSize.Width + 4f, 16f);
                    g.FillRectangle(badgeBack, badgeRect);
                    g.DrawString(badge, badgeFont, badgeText, badgeRect.X + 2f, badgeRect.Y + 1.5f);
                }
            }

            using (var centerFormat = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
            {
                if (unit == null)
                {
                    using (var emptyFont = new Font("Segoe UI", 9f, FontStyle.Italic))
                    using (var emptyBrush = new SolidBrush(Color.Gray))
                    {
                        g.DrawString(emptyText, emptyFont, emptyBrush, new RectangleF(0, 0, Width, Height), centerFormat);
                    }
                    return;
                }

                UnitId u = unit.Value;
                const float labelHeight = 36f;
                var iconArea = new RectangleF(8f, 22f, Width - 16f, Math.Max(10f, Height - 22f - labelHeight - 6f));
                float radius = Math.Max(8f, Math.Min(iconArea.Width, iconArea.Height) / 2f - 4f);
                float cx = iconArea.X + iconArea.Width / 2f;
                float cy = iconArea.Y + iconArea.Height / 2f;

                DrawUnitIcon(g, u, cx, cy, radius);

                using (var nameFont = new Font("Segoe UI", 9f, FontStyle.Bold))
                using (var typeFont = new Font("Segoe UI", 8.5f))
                using (var nameBrush = new SolidBrush(Color.White))
                using (var typeBrush = new SolidBrush(Color.Silver))
                {
                    var nameRect = new RectangleF(0, Height - labelHeight - 2f, Width, labelHeight / 2f);
                    var typeRect = new RectangleF(0, Height - labelHeight / 2f - 2f, Width, labelHeight / 2f);
                    g.DrawString(u.Color.ToString(), nameFont, nameBrush, nameRect, centerFormat);
                    g.DrawString(u.Type.ToString(), typeFont, typeBrush, typeRect, centerFormat);
                }
            }
        }

        private static void DrawUnitIcon(Graphics g, UnitId u, float cx, float cy, float radius)
        {
            Color fill = ToDrawingColor(u.Color);

            using (var outlinePen = new Pen(Color.White, 2f))
            using (var fillBrush = new SolidBrush(fill))
            {
                if (u.Type == ShapeType.Line)
                {
                    using (var linePen = new Pen(fill, 7f) { StartCap = LineCap.Round, EndCap = LineCap.Round })
                    {
                        g.DrawLine(linePen, cx - radius, cy, cx + radius, cy);
                    }
                    return;
                }

                int sides = GetSideCount(u.Type);

                double startAngle = sides % 2 == 1 ? -90.0 : -90.0 + 180.0 / sides;
                var points = new PointF[sides];

                for (int i = 0; i < sides; i++)
                {
                    double angle = (startAngle + i * 360.0 / sides) * Math.PI / 180.0;
                    points[i] = new PointF(
                        cx + (float)(Math.Cos(angle) * radius),
                        cy + (float)(Math.Sin(angle) * radius));
                }

                g.FillPolygon(fillBrush, points);
                g.DrawPolygon(outlinePen, points);
            }
        }

        internal static int GetSideCount(ShapeType type)
        {
            switch (type)
            {
                case ShapeType.Triangle: return 3;
                case ShapeType.Square: return 4;
                case ShapeType.Pentagon: return 5;
                case ShapeType.Hexagon: return 6;
                case ShapeType.Heptagon: return 7;
                case ShapeType.Octagon: return 8;
                default: return 2;
            }
        }

        internal static Color ToDrawingColor(ShapeColor color)
        {
            switch (color)
            {
                case ShapeColor.Black: return Color.FromArgb(15, 15, 15);
                case ShapeColor.White: return Color.WhiteSmoke;
                case ShapeColor.Red: return Color.FromArgb(225, 50, 50);
                case ShapeColor.Purple: return Color.MediumPurple;
                case ShapeColor.Cyan: return Color.Cyan;
                case ShapeColor.Green: return Color.LimeGreen;
                case ShapeColor.Blue: return Color.RoyalBlue;
                case ShapeColor.Yellow: return Color.Gold;
                default: return Color.Gray;
            }
        }
    }
}