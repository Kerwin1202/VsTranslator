using System.ComponentModel;
using System.Windows.Forms;

namespace VsTranslator.Settings
{
    partial class TranslateOptionsControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnSwap = new System.Windows.Forms.Button();
            this.cbTargetLanguage = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cbSourceLanguage = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cbServices = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lbBaidu = new System.Windows.Forms.LinkLabel();
            this.txtClientSecretBaidu = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtAppKeyBaidu = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.lbBing = new System.Windows.Forms.LinkLabel();
            this.txtClientSecretBing = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtAppKeyBing = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnSwap);
            this.groupBox1.Controls.Add(this.cbTargetLanguage);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.cbSourceLanguage);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.cbServices);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(3, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(364, 108);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Translating Service";
            // 
            // btnSwap
            // 
            this.btnSwap.Location = new System.Drawing.Point(303, 48);
            this.btnSwap.Name = "btnSwap";
            this.btnSwap.Size = new System.Drawing.Size(50, 48);
            this.btnSwap.TabIndex = 6;
            this.btnSwap.Text = "Swap";
            this.btnSwap.UseVisualStyleBackColor = true;
            this.btnSwap.Click += new System.EventHandler(this.btnSwap_Click);
            // 
            // cbTargetLanguage
            // 
            this.cbTargetLanguage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbTargetLanguage.FormattingEnabled = true;
            this.cbTargetLanguage.Location = new System.Drawing.Point(124, 76);
            this.cbTargetLanguage.Name = "cbTargetLanguage";
            this.cbTargetLanguage.Size = new System.Drawing.Size(173, 20);
            this.cbTargetLanguage.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(17, 80);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(95, 12);
            this.label3.TabIndex = 4;
            this.label3.Text = "Target Language";
            // 
            // cbSourceLanguage
            // 
            this.cbSourceLanguage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbSourceLanguage.FormattingEnabled = true;
            this.cbSourceLanguage.Location = new System.Drawing.Point(124, 48);
            this.cbSourceLanguage.Name = "cbSourceLanguage";
            this.cbSourceLanguage.Size = new System.Drawing.Size(173, 20);
            this.cbSourceLanguage.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(17, 53);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(95, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "Source Language";
            // 
            // cbServices
            // 
            this.cbServices.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbServices.FormattingEnabled = true;
            this.cbServices.Location = new System.Drawing.Point(124, 20);
            this.cbServices.Name = "cbServices";
            this.cbServices.Size = new System.Drawing.Size(229, 20);
            this.cbServices.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(17, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "Service";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.lbBaidu);
            this.groupBox2.Controls.Add(this.txtClientSecretBaidu);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.txtAppKeyBaidu);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Location = new System.Drawing.Point(3, 114);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(364, 83);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Baidu Translator Settings";
            // 
            // lbBaidu
            // 
            this.lbBaidu.AutoSize = true;
            this.lbBaidu.Location = new System.Drawing.Point(204, 0);
            this.lbBaidu.Name = "lbBaidu";
            this.lbBaidu.Size = new System.Drawing.Size(143, 12);
            this.lbBaidu.TabIndex = 4;
            this.lbBaidu.TabStop = true;
            this.lbBaidu.Text = "you can get it,click me";
            this.lbBaidu.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lbBaidu_LinkClicked);
            // 
            // txtClientSecretBaidu
            // 
            this.txtClientSecretBaidu.Location = new System.Drawing.Point(124, 50);
            this.txtClientSecretBaidu.Name = "txtClientSecretBaidu";
            this.txtClientSecretBaidu.Size = new System.Drawing.Size(229, 21);
            this.txtClientSecretBaidu.TabIndex = 3;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(17, 56);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(83, 12);
            this.label5.TabIndex = 2;
            this.label5.Text = "Client Secret";
            // 
            // txtAppKeyBaidu
            // 
            this.txtAppKeyBaidu.Location = new System.Drawing.Point(124, 20);
            this.txtAppKeyBaidu.Name = "txtAppKeyBaidu";
            this.txtAppKeyBaidu.Size = new System.Drawing.Size(229, 21);
            this.txtAppKeyBaidu.TabIndex = 1;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(17, 28);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(47, 12);
            this.label4.TabIndex = 0;
            this.label4.Text = "App Key";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.lbBing);
            this.groupBox3.Controls.Add(this.txtClientSecretBing);
            this.groupBox3.Controls.Add(this.label7);
            this.groupBox3.Controls.Add(this.txtAppKeyBing);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Location = new System.Drawing.Point(3, 203);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(364, 86);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Bing Translator Settings";
            // 
            // lbBing
            // 
            this.lbBing.AutoSize = true;
            this.lbBing.Location = new System.Drawing.Point(204, 0);
            this.lbBing.Name = "lbBing";
            this.lbBing.Size = new System.Drawing.Size(143, 12);
            this.lbBing.TabIndex = 4;
            this.lbBing.TabStop = true;
            this.lbBing.Text = "you can get it,click me";
            this.lbBing.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lbBing_LinkClicked);
            // 
            // txtClientSecretBing
            // 
            this.txtClientSecretBing.Location = new System.Drawing.Point(124, 52);
            this.txtClientSecretBing.Name = "txtClientSecretBing";
            this.txtClientSecretBing.Size = new System.Drawing.Size(229, 21);
            this.txtClientSecretBing.TabIndex = 3;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(17, 56);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(83, 12);
            this.label7.TabIndex = 2;
            this.label7.Text = "Client Secret";
            // 
            // txtAppKeyBing
            // 
            this.txtAppKeyBing.Location = new System.Drawing.Point(124, 22);
            this.txtAppKeyBing.Name = "txtAppKeyBing";
            this.txtAppKeyBing.Size = new System.Drawing.Size(229, 21);
            this.txtAppKeyBing.TabIndex = 1;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(17, 27);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(47, 12);
            this.label6.TabIndex = 0;
            this.label6.Text = "App Key";
            // 
            // TranslateOptionsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "TranslateOptionsControl";
            this.Size = new System.Drawing.Size(374, 301);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private GroupBox groupBox1;
        private ComboBox cbTargetLanguage;
        private Label label3;
        private ComboBox cbSourceLanguage;
        private Label label2;
        private Label label1;
        private Button btnSwap;
        private GroupBox groupBox2;
        private Label label4;
        private Label label5;
        private TextBox txtAppKeyBaidu;
        private TextBox txtClientSecretBaidu;
        private GroupBox groupBox3;
        private TextBox txtClientSecretBing;
        private Label label7;
        private TextBox txtAppKeyBing;
        private Label label6;
        private ComboBox cbServices;
        private LinkLabel lbBaidu;
        private LinkLabel lbBing;
    }
}
