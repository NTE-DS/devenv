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
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace NasuTek.DevEnvironment.Extensibility.Workbench.Docking
{
    partial class DockPane
    {
        private class SplitterControl : Control, ISplitterDragSource
        {
            DockPane m_pane;

            public SplitterControl(DockPane pane)
            {
                SetStyle(ControlStyles.Selectable, false);
                m_pane = pane;
            }

            public DockPane DockPane
            {
                get { return m_pane; }
            }

            private DockAlignment m_alignment;
            public DockAlignment Alignment
            {
                get { return m_alignment; }
                set
                {
                    m_alignment = value;
                    if (m_alignment == DockAlignment.Left || m_alignment == DockAlignment.Right)
                        Cursor = Cursors.VSplit;
                    else if (m_alignment == DockAlignment.Top || m_alignment == DockAlignment.Bottom)
                        Cursor = Cursors.HSplit;
                    else
                        Cursor = Cursors.Default;

                    if (DockPane.DockState == DockState.Document)
                        Invalidate();
                }
            }

            protected override void OnPaint(PaintEventArgs e)
            {
                base.OnPaint(e);

                if (DockPane.DockState != DockState.Document)
                    return;

                Graphics g = e.Graphics;
                Rectangle rect = ClientRectangle;
                if (Alignment == DockAlignment.Top || Alignment == DockAlignment.Bottom)
                    g.DrawLine(SystemPens.ControlDark, rect.Left, rect.Bottom - 1, rect.Right, rect.Bottom - 1);
                else if (Alignment == DockAlignment.Left || Alignment == DockAlignment.Right)
                    g.DrawLine(SystemPens.ControlDarkDark, rect.Right - 1, rect.Top, rect.Right - 1, rect.Bottom);
            }

            protected override void OnMouseDown(MouseEventArgs e)
            {
                base.OnMouseDown(e);

                if (e.Button != MouseButtons.Left)
                    return;

                DockPane.DockPanel.BeginDrag(this, Parent.RectangleToScreen(Bounds));
            }

            #region ISplitterDragSource Members

            void ISplitterDragSource.BeginDrag(Rectangle rectSplitter)
            {
            }

            void ISplitterDragSource.EndDrag()
            {
            }

            bool ISplitterDragSource.IsVertical
            {
                get
                {
                    NestedDockingStatus status = DockPane.NestedDockingStatus;
                    return (status.DisplayingAlignment == DockAlignment.Left ||
                        status.DisplayingAlignment == DockAlignment.Right);
                }
            }

            Rectangle ISplitterDragSource.DragLimitBounds
            {
                get
                {
                    NestedDockingStatus status = DockPane.NestedDockingStatus;
                    Rectangle rectLimit = Parent.RectangleToScreen(status.LogicalBounds);
                    if (((ISplitterDragSource)this).IsVertical)
                    {
                        rectLimit.X += MeasurePane.MinSize;
                        rectLimit.Width -= 2 * MeasurePane.MinSize;
                    }
                    else
                    {
                        rectLimit.Y += MeasurePane.MinSize;
                        rectLimit.Height -= 2 * MeasurePane.MinSize;
                    }

                    return rectLimit;
                }
            }

            void ISplitterDragSource.MoveSplitter(int offset)
            {
                NestedDockingStatus status = DockPane.NestedDockingStatus;
                double proportion = status.Proportion;
                if (status.LogicalBounds.Width <= 0 || status.LogicalBounds.Height <= 0)
                    return;
                else if (status.DisplayingAlignment == DockAlignment.Left)
                    proportion += ((double)offset) / (double)status.LogicalBounds.Width;
                else if (status.DisplayingAlignment == DockAlignment.Right)
                    proportion -= ((double)offset) / (double)status.LogicalBounds.Width;
                else if (status.DisplayingAlignment == DockAlignment.Top)
                    proportion += ((double)offset) / (double)status.LogicalBounds.Height;
                else
                    proportion -= ((double)offset) / (double)status.LogicalBounds.Height;

                DockPane.SetNestedDockingProportion(proportion);
            }

            #region IDragSource Members

            Control IDragSource.DragControl
            {
                get { return this; }
            }

            #endregion

            #endregion
        }

        private SplitterControl m_splitter;
        private SplitterControl Splitter
        {
            get { return m_splitter; }
        }

        internal Rectangle SplitterBounds
        {
            set { Splitter.Bounds = value; }
        }

        internal DockAlignment SplitterAlignment
        {
            set { Splitter.Alignment = value; }
        }
    }
}