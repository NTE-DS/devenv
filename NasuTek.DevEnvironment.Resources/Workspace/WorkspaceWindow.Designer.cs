namespace NasuTek.DevEnvironment.Workbench
{
    partial class WorkspaceWindow
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            NasuTek.DevEnvironment.Extendability.Workbench.Docking.DockPanelSkin dockPanelSkin1 = new NasuTek.DevEnvironment.Extendability.Workbench.Docking.DockPanelSkin();
            NasuTek.DevEnvironment.Extendability.Workbench.Docking.AutoHideStripSkin autoHideStripSkin1 = new NasuTek.DevEnvironment.Extendability.Workbench.Docking.AutoHideStripSkin();
            NasuTek.DevEnvironment.Extendability.Workbench.Docking.DockPanelGradient dockPanelGradient1 = new NasuTek.DevEnvironment.Extendability.Workbench.Docking.DockPanelGradient();
            NasuTek.DevEnvironment.Extendability.Workbench.Docking.TabGradient tabGradient1 = new NasuTek.DevEnvironment.Extendability.Workbench.Docking.TabGradient();
            NasuTek.DevEnvironment.Extendability.Workbench.Docking.DockPaneStripSkin dockPaneStripSkin1 = new NasuTek.DevEnvironment.Extendability.Workbench.Docking.DockPaneStripSkin();
            NasuTek.DevEnvironment.Extendability.Workbench.Docking.DockPaneStripGradient dockPaneStripGradient1 = new NasuTek.DevEnvironment.Extendability.Workbench.Docking.DockPaneStripGradient();
            NasuTek.DevEnvironment.Extendability.Workbench.Docking.TabGradient tabGradient2 = new NasuTek.DevEnvironment.Extendability.Workbench.Docking.TabGradient();
            NasuTek.DevEnvironment.Extendability.Workbench.Docking.DockPanelGradient dockPanelGradient2 = new NasuTek.DevEnvironment.Extendability.Workbench.Docking.DockPanelGradient();
            NasuTek.DevEnvironment.Extendability.Workbench.Docking.TabGradient tabGradient3 = new NasuTek.DevEnvironment.Extendability.Workbench.Docking.TabGradient();
            NasuTek.DevEnvironment.Extendability.Workbench.Docking.DockPaneStripToolWindowGradient dockPaneStripToolWindowGradient1 = new NasuTek.DevEnvironment.Extendability.Workbench.Docking.DockPaneStripToolWindowGradient();
            NasuTek.DevEnvironment.Extendability.Workbench.Docking.TabGradient tabGradient4 = new NasuTek.DevEnvironment.Extendability.Workbench.Docking.TabGradient();
            NasuTek.DevEnvironment.Extendability.Workbench.Docking.TabGradient tabGradient5 = new NasuTek.DevEnvironment.Extendability.Workbench.Docking.TabGradient();
            NasuTek.DevEnvironment.Extendability.Workbench.Docking.DockPanelGradient dockPanelGradient3 = new NasuTek.DevEnvironment.Extendability.Workbench.Docking.DockPanelGradient();
            NasuTek.DevEnvironment.Extendability.Workbench.Docking.TabGradient tabGradient6 = new NasuTek.DevEnvironment.Extendability.Workbench.Docking.TabGradient();
            NasuTek.DevEnvironment.Extendability.Workbench.Docking.TabGradient tabGradient7 = new NasuTek.DevEnvironment.Extendability.Workbench.Docking.TabGradient();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.dockPanel1 = new NasuTek.DevEnvironment.Extendability.Workbench.Docking.DockPanel();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.Location = new System.Drawing.Point(0, 595);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(967, 22);
            this.statusStrip1.TabIndex = 0;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.SystemColors.Control;
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(967, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            this.menuStrip1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.menuStrip1_MouseUp);
            // 
            // dockPanel1
            // 
            this.dockPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dockPanel1.Location = new System.Drawing.Point(0, 24);
            this.dockPanel1.Name = "dockPanel1";
            this.dockPanel1.Size = new System.Drawing.Size(967, 571);
            dockPanelGradient1.EndColor = System.Drawing.SystemColors.ControlLight;
            dockPanelGradient1.StartColor = System.Drawing.SystemColors.ControlLight;
            autoHideStripSkin1.DockStripGradient = dockPanelGradient1;
            tabGradient1.EndColor = System.Drawing.SystemColors.Control;
            tabGradient1.StartColor = System.Drawing.SystemColors.Control;
            tabGradient1.TextColor = System.Drawing.SystemColors.ControlDarkDark;
            autoHideStripSkin1.TabGradient = tabGradient1;
            autoHideStripSkin1.TextFont = new System.Drawing.Font("Segoe UI", 9F);
            dockPanelSkin1.AutoHideStripSkin = autoHideStripSkin1;
            tabGradient2.EndColor = System.Drawing.SystemColors.ControlLightLight;
            tabGradient2.StartColor = System.Drawing.SystemColors.ControlLightLight;
            tabGradient2.TextColor = System.Drawing.SystemColors.ControlText;
            dockPaneStripGradient1.ActiveTabGradient = tabGradient2;
            dockPanelGradient2.EndColor = System.Drawing.SystemColors.Control;
            dockPanelGradient2.StartColor = System.Drawing.SystemColors.Control;
            dockPaneStripGradient1.DockStripGradient = dockPanelGradient2;
            tabGradient3.EndColor = System.Drawing.SystemColors.ControlLight;
            tabGradient3.StartColor = System.Drawing.SystemColors.ControlLight;
            tabGradient3.TextColor = System.Drawing.SystemColors.ControlText;
            dockPaneStripGradient1.InactiveTabGradient = tabGradient3;
            dockPaneStripSkin1.DocumentGradient = dockPaneStripGradient1;
            dockPaneStripSkin1.TextFont = new System.Drawing.Font("Segoe UI", 9F);
            tabGradient4.EndColor = System.Drawing.SystemColors.ActiveCaption;
            tabGradient4.LinearGradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            tabGradient4.StartColor = System.Drawing.SystemColors.GradientActiveCaption;
            tabGradient4.TextColor = System.Drawing.SystemColors.ActiveCaptionText;
            dockPaneStripToolWindowGradient1.ActiveCaptionGradient = tabGradient4;
            tabGradient5.EndColor = System.Drawing.SystemColors.Control;
            tabGradient5.StartColor = System.Drawing.SystemColors.Control;
            tabGradient5.TextColor = System.Drawing.SystemColors.ControlText;
            dockPaneStripToolWindowGradient1.ActiveTabGradient = tabGradient5;
            dockPanelGradient3.EndColor = System.Drawing.SystemColors.ControlLight;
            dockPanelGradient3.StartColor = System.Drawing.SystemColors.ControlLight;
            dockPaneStripToolWindowGradient1.DockStripGradient = dockPanelGradient3;
            tabGradient6.EndColor = System.Drawing.SystemColors.InactiveCaption;
            tabGradient6.LinearGradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            tabGradient6.StartColor = System.Drawing.SystemColors.GradientInactiveCaption;
            tabGradient6.TextColor = System.Drawing.SystemColors.InactiveCaptionText;
            dockPaneStripToolWindowGradient1.InactiveCaptionGradient = tabGradient6;
            tabGradient7.EndColor = System.Drawing.Color.Transparent;
            tabGradient7.StartColor = System.Drawing.Color.Transparent;
            tabGradient7.TextColor = System.Drawing.SystemColors.ControlDarkDark;
            dockPaneStripToolWindowGradient1.InactiveTabGradient = tabGradient7;
            dockPaneStripSkin1.ToolWindowGradient = dockPaneStripToolWindowGradient1;
            dockPanelSkin1.DockPaneStripSkin = dockPaneStripSkin1;
            this.dockPanel1.Skin = dockPanelSkin1;
            this.dockPanel1.TabIndex = 2;
            // 
            // WorkspaceWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(967, 617);
            this.Controls.Add(this.dockPanel1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "WorkspaceWindow";
            this.Text = "WorkspaceWindow";
            this.Load += new System.EventHandler(this.Workspace_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private NasuTek.DevEnvironment.Extendability.Workbench.Docking.DockPanel dockPanel1;
    }
}