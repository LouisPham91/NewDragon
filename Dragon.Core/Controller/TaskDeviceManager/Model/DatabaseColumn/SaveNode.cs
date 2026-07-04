using System;

namespace Dragon.Controller.TaskDeviceManager.Model.DatabaseColumn
{
    public class SaveNode
    {
        private static string Clean(string input)
        {
            return input?.Replace("\r", string.Empty) ?? string.Empty;
        }

        private string _className = string.Empty;
        public string ClassName
        {
            get => _className;
            set => _className = Clean(value);
        }

        private string _text = string.Empty;
        public string Text
        {
            get => _text;
            set => _text = Clean(value);
        }

        private string _resourceName = string.Empty;
        public string ResourceName
        {
            get => _resourceName;
            set => _resourceName = Clean(value);
        }

        private string _packageName = string.Empty;
        public string PackageName
        {
            get => _packageName;
            set => _packageName = Clean(value);
        }

        private string _contentDescription = string.Empty;
        public string ContentDescription
        {
            get => _contentDescription;
            set => _contentDescription = Clean(value);
        }

        public int Index { get; set; }

        public override string ToString()
        {
            return $"{Text},{ContentDescription}";
        }

        public SaveNode UpdateFromNode(SaveNode node)
        {
            ClassName = node.ClassName;
            Text = node.Text;
            ResourceName = node.ResourceName;
            PackageName = node.PackageName;
            ContentDescription = node.ContentDescription;
            Index = node.Index;
            return this;
        }
    }
}
