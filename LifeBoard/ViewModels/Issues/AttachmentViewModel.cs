namespace LifeBoard.ViewModels.Issues
{
    /// <summary>
    /// Class AttachmentViewModel
    /// </summary>
    public class AttachmentViewModel : ParentViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AttachmentViewModel" /> class.
        /// </summary>
        /// <param name="parent">The parent.</param>
        public AttachmentViewModel(object parent) : base(parent)
        {
        }

        /// <summary>
        /// Gets or sets the name of the file.
        /// </summary>
        /// <value>The name of the file.</value>
        public string FileName { get; set; }
    }
}