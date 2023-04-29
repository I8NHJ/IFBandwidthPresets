using SDRSharp.Common;
using SDRSharp.Radio;
using System;
using System.IO;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.Xml;
using System.Configuration;
using System.Drawing;
using Telerik.WinControls.Drawing;

namespace SDRSharp.Plugin.Diagnostics
{
    public class ButtonConfiguration
    {
        public string ButtonText { get; set; }
        public int Bandwidth { get; set; }
        public int FilterOrder { get; set; }
        public int FilterType { get; set; }
    }

    public class ButtonConfigRoot
    {
        public ButtonConfiguration[] Values;
    }

    public partial class BandwidthPanel : UserControl
    {
        private TableLayoutPanel mainTableLayoutPanel;
        private ISharpControl _control;

        public BandwidthPanel(ISharpControl control)
        {
            _control = control;

            InitializeComponent();

            InitializeButtons();
        }

        private void InitializeButtons()
        {
            var xs = new XmlSerializer(typeof(ButtonConfigRoot));
            var reader = new XmlTextReader("Plugins/IFBandwidthPresets.xml");
            var root = (ButtonConfigRoot)xs.Deserialize(reader);
            reader.Close();

            int numberOfButtons = root.Values.Length;

            int numberOfColumns = 3;

            int numberOfLines = (int)Math.Ceiling(numberOfButtons / (float)numberOfColumns);


            mainTableLayoutPanel.SuspendLayout();

            mainTableLayoutPanel.RowCount = numberOfLines;

            for (int i = 0; i < numberOfLines; i++)
            {
                mainTableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            }


            var button = new Button();
            button.UseVisualStyleBackColor = true;
            button.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom;
            button.Location = new System.Drawing.Point(171, 26);
            button.Size = new System.Drawing.Size(80, 23);
            button.Text = "Config"; //MM 
            button.TextAlign = ContentAlignment.MiddleCenter; //MM
            button.Click += Button_Click;
            mainTableLayoutPanel.Controls.Add(button, 0, 0);


            for (int i = 1; i < numberOfButtons; i++)
            {
                var radiobutton = new RadioButton();
                radiobutton.UseVisualStyleBackColor = true;
                radiobutton.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom;
                radiobutton.Location = new System.Drawing.Point(171, 26);
                radiobutton.Size = new System.Drawing.Size(80, 23);
                radiobutton.Text = root.Values[i].ButtonText; //MM 
                radiobutton.Appearance = Appearance.Button; //MM
                radiobutton.ForeColor = Color.Red;
                radiobutton.Font = new Font("Calibri",10);
                radiobutton.TextAlign = ContentAlignment.MiddleCenter; //MM
                //Button.Text = Utils.GetFrequencyDisplayRounded(root.Values[i].Bandwidth);
                radiobutton.Tag = root.Values[i];
                radiobutton.Click += RadioButton_Click;

                mainTableLayoutPanel.Controls.Add(radiobutton, i % numberOfColumns, i / numberOfColumns);

            }

            mainTableLayoutPanel.ResumeLayout(false);
        }

        private void Button_Click(object sender, EventArgs e)
        {
        }

        private void RadioButton_Click(object sender, EventArgs e)
        {
            var config = (ButtonConfiguration)(sender as RadioButton).Tag;
            var radiobutton = sender as RadioButton;
            _control.FilterBandwidth = config.Bandwidth;
            _control.FilterOrder = config.FilterOrder;
            _control.FilterType = (WindowType)Enum.ToObject(typeof(WindowType), config.FilterType);
            radiobutton.Font = new Font("Calibri",10,FontStyle.Italic);
        }

        private void InitializeComponent()
        {
            this.mainTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.mainTableLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainTableLayoutPanel
            // 
            this.mainTableLayoutPanel.ColumnCount = 3;
            this.mainTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.mainTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.mainTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.mainTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainTableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.mainTableLayoutPanel.Name = "tableLayoutPanel1";
            this.mainTableLayoutPanel.Size = new System.Drawing.Size(254, 150);
            this.mainTableLayoutPanel.TabIndex = 0;
            // 
            // BandwidthPanel
            // 
            this.Controls.Add(this.mainTableLayoutPanel);
            this.Name = "BandwidthPanel";
            this.Size = new System.Drawing.Size(254, 150);
            this.mainTableLayoutPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }
    }
}
