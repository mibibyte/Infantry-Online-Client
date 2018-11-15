using System;
using System.Windows.Forms;

namespace FreeInfantryClient.Windows.Account
{
    public partial class ReminderForm : Form
    {
        /// <summary>
        /// Gets our changed reminder text
        /// </summary>
        public string Reminder
        {
            get;
            private set;
        }

        public ReminderForm(string reminder)
        {
            InitializeComponent();

            AcceptButton = ReminderOkButton;
            CancelButton = ReminderCancelButton;

            SavedReminderLabel.Text = reminder;

            Reminder = string.Empty;
        }

        private void ReminderOkButton_Click(object sender, EventArgs e)
        {
            Reminder = ReminderTextBox.Text;
            Close();
        }

        private void ReminderCancelButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void ReminderForm_Load(object sender, EventArgs e)
        {

        }
    }
}