using NasuTek.DevEnvironment.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NasuTek.DevEnvironment.Resources.ProjectAPI;

namespace NasuTek.DevEnvironment
{
    public partial class SolutionExplorer : DevEnvPane {
        private IProject m_ActiveProject;

        public SolutionExplorer()
        {
            InitializeComponent();
        }

        public void SetActiveProject(IProject proj) {
            m_ActiveProject = proj;

            treeView1.Nodes.Clear();

            var rootNode = new TreeNode(m_ActiveProject.ProjectName) { ImageKey = "GenericProject", SelectedImageKey = "GenericProject", Tag = m_ActiveProject };
            FillFolder(rootNode, m_ActiveProject.RootFolder, m_ActiveProject);

            treeView1.Nodes.Add(rootNode);
        }

        private void FillFolder(TreeNode rootNode, IFolder folder, IProject proj) {
            foreach (var objNode in folder.Objects.Select(o => new TreeNode(o.Name) {ImageKey = "GenericFile", SelectedImageKey = "GenericFile", Tag = Tuple.Create(o, proj)})) {
                rootNode.Nodes.Add(objNode);
            }

            foreach (var subFolder in folder.SubFolders) {
                var folderNode = new TreeNode(subFolder.Name) { ImageKey = "GenericFolder", SelectedImageKey = "GenericFolder", Tag = Tuple.Create(subFolder, proj) };
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
            if (!(((TreeNode) e.Item).Tag is IProject))
                DoDragDrop(e.Item, DragDropEffects.Move);
        }

        private void treeView1_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        private void treeView1_DragDrop(object sender, DragEventArgs e)
        {
            TreeNode NewNode;

            if (e.Data.GetDataPresent("System.Windows.Forms.TreeNode", false))
            {
                Point pt = ((TreeView)sender).PointToClient(new Point(e.X, e.Y));
                TreeNode DestinationNode = ((TreeView)sender).GetNodeAt(pt);
                NewNode = (TreeNode)e.Data.GetData("System.Windows.Forms.TreeNode");
                if (DestinationNode.Tag is Tuple<IObject, IProject>) return;

                if (NewNode.Tag is Tuple<IObject, IProject>)
                {
                    if (!MoveFile(NewNode, DestinationNode, ((Tuple<IObject, IProject>)NewNode.Tag).Item2)) return;
                    DestinationNode.Nodes.Add((TreeNode)NewNode.Clone());
                    DestinationNode.Expand();
                    //Remove Original Node
                    NewNode.Remove();
                }

                if (NewNode.Tag is Tuple<IFolder,IProject>) {
                    if (!MoveFolder(NewNode, DestinationNode, ((Tuple<IFolder, IProject>) NewNode.Tag).Item2)) return;
                    DestinationNode.Nodes.Add((TreeNode)NewNode.Clone());
                    DestinationNode.Expand();
                    //Remove Original Node
                    NewNode.Remove();
                }
            }
        }


    }
}
