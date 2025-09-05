namespace MovingObjectClient_Yuki
{
    partial class ClientForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // ClientForm
            this.ClientSize = new System.Drawing.Size(600, 350);
            this.Name = "ClientForm";
            this.Text = "Client";
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.ClientForm_Paint);
            this.ResumeLayout(false);
        }

        #endregion
    }
}
