/***************************************************************************************************
 * NasuTek Developer Studio Development Environment Core DLL
 * Copyright (C) 2005-2014 NasuTek Enterprises
 * Window Docking Portions Copyright (C) 2007-2012 Weifen Luo (email: weifenluo@yahoo.com)
 * Addin Engine Portions Copyright (C) 2001-2012 AlphaSierraPapa for the SharpDevelop Team
 *
 * This library is free software; you can redistribute it and/or modify it under the terms of the 
 * GNU Library General Public License as published by the Free Software Foundation; either
 * version 2 of the License, or (at your option) any later version.
 *
 * This library is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; 
 * without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 * See the GNU Library General Public License for more details.
 *
 * You should have received a copy of the GNU Library General Public License along with this 
 * library; if not, write to the Free Software Foundation, Inc., 59 Temple Place - Suite 330,
 * Boston, MA 02111-1307, USA.
 ***************************************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.ComponentModel;

namespace NasuTek.DevEnvironment.Extensibility.Workbench.Docking
{
    partial class DockPanel
    {
        private sealed class SplitterDragHandler : DragHandler
        {
            private class SplitterOutline
            {
                public SplitterOutline()
                {
                    m_dragForm = new DragForm();
                    SetDragForm(Rectangle.Empty);
                    DragForm.BackColor = Color.Black;
                    DragForm.Opacity = 0.7;
                    DragForm.Show(false);
                }

                DragForm m_dragForm;
                private DragForm DragForm
                {
                    get { return m_dragForm; }
                }

                public void Show(Rectangle rect)
                {
                    SetDragForm(rect);
                }

                public void Close()
                {
                    DragForm.Close();
                }

                private void SetDragForm(Rectangle rect)
                {
                    DragForm.Bounds = rect;
                    if (rect == Rectangle.Empty)
                        DragForm.Region = new Region(Rectangle.Empty);
                    else if (DragForm.Region != null)
                        DragForm.Region = null;
                }
            }

            public SplitterDragHandler(DockPanel dockPanel)
                : base(dockPanel)
            {
            }

            public new ISplitterDragSource DragSource
            {
                get { return base.DragSource as ISplitterDragSource; }
                private set { base.DragSource = value; }
            }

            private SplitterOutline m_outline;
            private SplitterOutline Outline
            {
                get { return m_outline; }
                set { m_outline = value; }
            }

            private Rectangle m_rectSplitter;
            private Rectangle RectSplitter
            {
                get { return m_rectSplitter; }
                set { m_rectSplitter = value; }
            }

            public void BeginDrag(ISplitterDragSource dragSource, Rectangle rectSplitter)
            {
                DragSource = dragSource;
                RectSplitter = rectSplitter;

                if (!BeginDrag())
                {
                    DragSource = null;
                    return;
                }

                Outline = new SplitterOutline();
                Outline.Show(rectSplitter);
                DragSource.BeginDrag(rectSplitter);
            }

            protected override void OnDragging()
            {
                Outline.Show(GetSplitterOutlineBounds(Control.MousePosition));
            }

            protected override void OnEndDrag(bool abort)
            {
                DockPanel.SuspendLayout(true);

                Outline.Close();

                if (!abort)
                    DragSource.MoveSplitter(GetMovingOffset(Control.MousePosition));

                DragSource.EndDrag();
                DockPanel.ResumeLayout(true, true);
            }

            private int GetMovingOffset(Point ptMouse)
            {
                Rectangle rect = GetSplitterOutlineBounds(ptMouse);
                if (DragSource.IsVertical)
                    return rect.X - RectSplitter.X;
                else
                    return rect.Y - RectSplitter.Y;
            }

            private Rectangle GetSplitterOutlineBounds(Point ptMouse)
            {
                Rectangle rectLimit = DragSource.DragLimitBounds;

                Rectangle rect = RectSplitter;
                if (rectLimit.Width <= 0 || rectLimit.Height <= 0)
                    return rect;

                if (DragSource.IsVertical)
                {
                    rect.X += ptMouse.X - StartMousePosition.X;
                    rect.Height = rectLimit.Height;
                }
                else
                {
                    rect.Y += ptMouse.Y - StartMousePosition.Y;
                    rect.Width = rectLimit.Width;
                }

                if (rect.Left < rectLimit.Left)
                    rect.X = rectLimit.X;
                if (rect.Top < rectLimit.Top)
                    rect.Y = rectLimit.Y;
                if (rect.Right > rectLimit.Right)
                    rect.X -= rect.Right - rectLimit.Right;
                if (rect.Bottom > rectLimit.Bottom)
                    rect.Y -= rect.Bottom - rectLimit.Bottom;

                return rect;
            }
        }

        private SplitterDragHandler m_splitterDragHandler = null;
        private SplitterDragHandler GetSplitterDragHandler()
        {
            if (m_splitterDragHandler == null)
                m_splitterDragHandler = new SplitterDragHandler(this);
            return m_splitterDragHandler;
        }

        internal void BeginDrag(ISplitterDragSource dragSource, Rectangle rectSplitter)
        {
            GetSplitterDragHandler().BeginDrag(dragSource, rectSplitter);
        }
    }
}
