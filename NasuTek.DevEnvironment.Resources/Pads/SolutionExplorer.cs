using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using NasuTek.DevEnvironment.Extensibility.Project;
using NasuTek.DevEnvironment.Extensibility.Workbench;
using NasuTek.DevEnvironment.Extensibility;

namespace NasuTek.DevEnvironment.Pads
{
    public partial class SolutionExplorer : DevEnvPane, IDevEnvSolutionSvc {
        private ISolution m_ActiveSolution;

        internal string Extension { get; set; }

        public bool HideSolutionRoot { get; set; }

        public ISolution ActiveSolution {
            get { return m_ActiveSolution; }
            set
            {
                DevEnv.GetActiveInstance().WorkspaceEnvironment.CloseAllDocuments();

                m_ActiveSolution = value;

                treeView1.Nodes.Clear();

                TreeNode rootNode = null;

                if (!HideSolutionRoot)
                    rootNode = new TreeNode("Solution '" + m_ActiveSolution.SolutionName + "'") { ImageKey = "GenericProject", SelectedImageKey = "GenericProject", Tag = m_ActiveSolution };

                RecurseFolder(m_ActiveSolution.RootFolder, rootNode);

                if (!HideSolutionRoot)
                    treeView1.Nodes.Add(rootNode);
            }
        }

        private void RecurseFolder(ISolutionFolder rootFolder, TreeNode rootNode) {
            foreach (var proj in rootFolder.Projects) {
                var projNode = new TreeNode(proj.ProjectName) { ImageKey = "GenericProject", SelectedImageKey = "GenericProject", Tag = proj };
                FillFolder(projNode, proj.RootFolder, proj);
                if (!HideSolutionRoot)
                    rootNode.Nodes.Add(projNode);
                else
                    treeView1.Nodes.Add(projNode);
            }

            foreach(var folder in rootFolder.SubFolders) {
                if (!HideSolutionRoot) {
                    var node = new TreeNode(folder.Name) { ImageKey = "GenericFolder", SelectedImageKey = "GenericFolder", Tag = folder };
                    RecurseFolder(folder, node);
                    rootNode.Nodes.Add(node);
                } else {
                    RecurseFolder(folder, rootNode);
                }
            }
        }

        public SolutionExplorer() {
            InitializeComponent();

            DevEnvSvc.RegisterService(DevEnvSvc.SolutionSvc, this);
        }

        private void FillFolder(TreeNode rootNode, IFolder folder, IProject proj) {
            foreach (var objNode in folder.Objects.Select(o => new TreeNode(o.Name) {ImageKey = "GenericFile", SelectedImageKey = "GenericFile", Tag = Tuple.Create(o, proj)})) {
                rootNode.Nodes.Add(objNode);
            }

            foreach (var subFolder in folder.SubFolders) {
                var folderNode = new TreeNode(subFolder.Name) {ImageKey = "GenericFolder", SelectedImageKey = "GenericFolder", Tag = Tuple.Create(subFolder, proj)};
                FillFolder(folderNode, subFolder, proj);
                rootNode.Nodes.Add(folderNode);
            }
        }

        private bool MoveFile(TreeNode sourceNode, TreeNode destNode, IProject project) {
            return project.MoveObject(((Tuple<IObject, IProject>) sourceNode.Tag).Item1, FolderOrProject(sourceNode.Parent.Tag), FolderOrProject(destNode.Tag));
        }

        private IFolder FolderOrProject(object p) {
            return p is IProject ? ((IProject) p).RootFolder : ((Tuple<IFolder, IProject>) p).Item1;
        }

        private bool MoveFolder(TreeNode sourceNode, TreeNode destNode, IProject project) {
            return project.MoveObject(((Tuple<IFolder, IProject>) sourceNode.Tag).Item1, FolderOrProject(sourceNode.Parent.Tag), FolderOrProject(destNode.Tag));
        }

        private void treeView1_ItemDrag(object sender, ItemDragEventArgs e) {
            if (!(((TreeNode)e.Item).Tag is IProject) && !typeof(ISolution).IsAssignableFrom(((TreeNode)e.Item).Tag.GetType()))
                DoDragDrop(e.Item, DragDropEffects.Move);
        }

        private void treeView1_DragEnter(object sender, DragEventArgs e) {
            e.Effect = DragDropEffects.Move;
        }

        private void treeView1_DragDrop(object sender, DragEventArgs e) {
            TreeNode NewNode;

            if (e.Data.GetDataPresent("System.Windows.Forms.TreeNode", false)) {
                Point pt = ((TreeView) sender).PointToClient(new Point(e.X, e.Y));
                TreeNode DestinationNode = ((TreeView) sender).GetNodeAt(pt);
                NewNode = (TreeNode) e.Data.GetData("System.Windows.Forms.TreeNode");
                if (DestinationNode.Tag is Tuple<IObject, IProject>) return;
                if (typeof(ISolution).IsAssignableFrom(DestinationNode.Tag.GetType())) return;

                if (NewNode.Tag is Tuple<IObject, IProject>) {
                    if (!MoveFile(NewNode, DestinationNode, ((Tuple<IObject, IProject>) NewNode.Tag).Item2)) return;
                    DestinationNode.Nodes.Add((TreeNode) NewNode.Clone());
                    DestinationNode.Expand();
                    //Remove Original Node
                    NewNode.Remove();
                }

                if (NewNode.Tag is Tuple<IFolder, IProject>) {
                    if (!MoveFolder(NewNode, DestinationNode, ((Tuple<IFolder, IProject>) NewNode.Tag).Item2)) return;
                    DestinationNode.Nodes.Add((TreeNode) NewNode.Clone());
                    DestinationNode.Expand();
                    //Remove Original Node
                    NewNode.Remove();
                }
            }
        }
        
        public void RemoveObject(TreeNode folderNode, TreeNode deleteNode, IObject objId, IProject project)
        {
            if (MessageBox.Show("Are you sure you want to remove \"" + objId.Name + "\"?", DevEnv.GetActiveInstance().Settings.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Stop, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

            deleteNode.Remove();
            ((Tuple<IFolder, IProject>)folderNode.Tag).Item1.RemoveObject(objId);
        }
        
        public void RemoveFolder(TreeNode folderNode, TreeNode deleteNode, IFolder folder, IProject project)
        {
            if (MessageBox.Show("Are you sure you want to remove \"" + folder.Name + "\"?", DevEnv.GetActiveInstance().Settings.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Stop, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;

            deleteNode.Remove();

            if (folderNode.Tag is Tuple<IFolder, IProject>)
                ((Tuple<IFolder, IProject>)folderNode.Tag).Item1.RemoveFolder(folder);
            else if (folderNode.Tag is IProject)
                ((IProject)folderNode.Tag).RootFolder.RemoveFolder(folder);
        }
        
        public void AddObject(TreeNode folderNode, IObject objId, IProject project) {
            folderNode.Nodes.Add(new TreeNode(objId.Name) {ImageKey = "GenericFile", SelectedImageKey = "GenericFile", Tag = Tuple.Create(objId, project)});
            ((Tuple<IFolder, IProject>) folderNode.Tag).Item1.AddObject(objId);
        }
        
        public void AddFolder(TreeNode folderNode, IFolder folder, IProject project) {
            folderNode.Nodes.Add(new TreeNode(folder.Name) {ImageKey = "GenericFolder", SelectedImageKey = "GenericFolder", Tag = Tuple.Create(folder, project)});
            if (folderNode.Tag is Tuple<IFolder, IProject>)
                ((Tuple<IFolder, IProject>) folderNode.Tag).Item1.AddFolder(folder);
            else if (folderNode.Tag is IProject)
                ((IProject) folderNode.Tag).RootFolder.AddFolder(folder);
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e) {
            if (treeView1.SelectedNode.Tag is IProject) {
                var propPad = (PropertyWindow) DevEnv.GetActiveInstance().Extensibility.GetPane("PropertyWindow");
                if (propPad == null) return;
                propPad.SetObjects(((IProject) treeView1.SelectedNode.Tag).PropertyObjects);
            }

            if (treeView1.SelectedNode.Tag is Tuple<IObject, IProject>) {
                var propPad = (PropertyWindow) DevEnv.GetActiveInstance().Extensibility.GetPane("PropertyWindow");
                if (propPad == null) return;
                propPad.SetObjects(((Tuple<IObject, IProject>) treeView1.SelectedNode.Tag).Item1.PropertyObjects);
            }

            if (treeView1.SelectedNode.Tag is Tuple<IFolder, IProject>) {
                var propPad = (PropertyWindow) DevEnv.GetActiveInstance().Extensibility.GetPane("PropertyWindow");
                if (propPad == null) return;
                propPad.SetObjects(((Tuple<IFolder, IProject>) treeView1.SelectedNode.Tag).Item1.PropertyObjects);
            }
        }

        private void treeView1_DoubleClick(object sender, EventArgs e) {
            if (treeView1.SelectedNode == null) return;
            if (!(treeView1.SelectedNode.Tag is Tuple<IObject, IProject>)) return;
            var obj = ((Tuple<IObject, IProject>) treeView1.SelectedNode.Tag);

            obj.Item2.OpenObject(obj.Item1);
        }

        private void treeView1_MouseUp(object sender, MouseEventArgs e) {
            if (e.Button != MouseButtons.Right) return;
            // Select the clicked node
            treeView1.SelectedNode = treeView1.GetNodeAt(e.X, e.Y);

            if (treeView1.SelectedNode == null) return;

            if (treeView1.SelectedNode.Tag is IProject && treeView1.SelectedNode.Tag is ISolutionExplorerRightClick) {
                var rightClick = (ISolutionExplorerRightClick) treeView1.SelectedNode.Tag;
                var myContextMenuStrip = rightClick.RightClick(treeView1.SelectedNode);
                if (myContextMenuStrip != null)
                    myContextMenuStrip.Show(treeView1, e.Location);
            }
            if (treeView1.SelectedNode.Tag is Tuple<IObject, IProject> && ((Tuple<IObject, IProject>) treeView1.SelectedNode.Tag).Item1 is ISolutionExplorerRightClick) {
                var rightClick = (ISolutionExplorerRightClick) ((Tuple<IObject, IProject>) treeView1.SelectedNode.Tag).Item1;
                var myContextMenuStrip = rightClick.RightClick(treeView1.SelectedNode);
                if (myContextMenuStrip != null)
                    myContextMenuStrip.Show(treeView1, e.Location);
            }
            if (treeView1.SelectedNode.Tag is Tuple<IFolder, IProject> && ((Tuple<IFolder, IProject>) treeView1.SelectedNode.Tag).Item1 is ISolutionExplorerRightClick) {
                var rightClick = (ISolutionExplorerRightClick) ((Tuple<IFolder, IProject>) treeView1.SelectedNode.Tag).Item1;
                var myContextMenuStrip = rightClick.RightClick(treeView1.SelectedNode);
                if (myContextMenuStrip != null)
                    myContextMenuStrip.Show(treeView1, e.Location);
            }
        }

        private void treeView1_AfterLabelEdit(object sender, NodeLabelEditEventArgs e) {
            if (e.Label == null) return;
            if (e.Label == "") {
                MessageBox.Show("Blank names not allowed.", DevEnv.GetActiveInstance().Settings.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                e.CancelEdit = true;
                return;
            }

            if (e.Node.Tag is IProject && e.Node.Tag is ISolutionExplorerRename) {
                var rightClick = (ISolutionExplorerRename) e.Node.Tag;
                if (!rightClick.Rename(e.Label)) {
                    MessageBox.Show("Cannot rename this object.", DevEnv.GetActiveInstance().Settings.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    e.CancelEdit = true;
                } else {
                    DevEnv.GetActiveInstance().WorkspaceEnvironment.RefreshDocuments();
                }
            } else if (e.Node.Tag is Tuple<IObject, IProject> && ((Tuple<IObject, IProject>) e.Node.Tag).Item1 is ISolutionExplorerRename) {
                var rightClick = (ISolutionExplorerRename) ((Tuple<IObject, IProject>) e.Node.Tag).Item1;
                if (!rightClick.Rename(e.Label)) {
                    MessageBox.Show("Cannot rename this object.", DevEnv.GetActiveInstance().Settings.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    e.CancelEdit = true;
                } else {
                    DevEnv.GetActiveInstance().WorkspaceEnvironment.RefreshDocuments();
                }
            } else if (e.Node.Tag is Tuple<IFolder, IProject> && ((Tuple<IFolder, IProject>) e.Node.Tag).Item1 is ISolutionExplorerRename) {
                var rightClick = (ISolutionExplorerRename) ((Tuple<IFolder, IProject>) e.Node.Tag).Item1;
                if (!rightClick.Rename(e.Label)) {
                    MessageBox.Show("Cannot rename this object.", DevEnv.GetActiveInstance().Settings.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    e.CancelEdit = true;
                } else {
                    DevEnv.GetActiveInstance().WorkspaceEnvironment.RefreshDocuments();
                }
            } else {
                MessageBox.Show("Cannot rename this object.", DevEnv.GetActiveInstance().Settings.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                e.CancelEdit = true;
            }
        }

        private void treeView1_KeyDown(object sender, KeyEventArgs e) {
            if (e.KeyCode != Keys.Delete) return;
            DeleteSelectedNode();
        }

        public void DeleteSelectedNode() {
            if (treeView1.SelectedNode == null) return;

            if (treeView1.SelectedNode.Tag is Tuple<IObject, IProject>) {
                if (((Tuple<IObject, IProject>) treeView1.SelectedNode.Tag).Item1.GetType().GetCustomAttributes(typeof (DeletableObjectAttribute), false).Length > 0)
                    RemoveObject(treeView1.SelectedNode.Parent, treeView1.SelectedNode, ((Tuple<IObject, IProject>) treeView1.SelectedNode.Tag).Item1, ((Tuple<IObject, IProject>) treeView1.SelectedNode.Tag).Item2);
            }

            if (treeView1.SelectedNode.Tag is Tuple<IFolder, IProject>) {
                if (((Tuple<IFolder, IProject>) treeView1.SelectedNode.Tag).Item1.GetType().GetCustomAttributes(typeof (DeletableObjectAttribute), false).Length > 0)
                    RemoveFolder(treeView1.SelectedNode.Parent, treeView1.SelectedNode, ((Tuple<IFolder, IProject>) treeView1.SelectedNode.Tag).Item1, ((Tuple<IFolder, IProject>) treeView1.SelectedNode.Tag).Item2);
            }
        }

        public void OpenDocument(DocumentMetadata meta)
        {
            DevEnv.GetActiveInstance().WorkspaceEnvironment.OpenDocument(meta);
        }
    }
}