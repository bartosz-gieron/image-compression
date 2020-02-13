using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace kompresja
{
    public partial class Form1 : Form
    {
        private int count = 0;

        public Form1()
        {
            InitializeComponent();
        }

        private void Opnbutton_Click_1(object sender, EventArgs e)
        {
            using (OpenFileDialog fld = new OpenFileDialog())
            {
                if (fld.ShowDialog() == DialogResult.OK)
                {
                    textBox.Text = fld.FileName.ToString();
                    Bitmap image = new Bitmap(fld.FileName);
                    pictureBox.Image = image;
                }
                else MessageBox.Show("nie wybrano pliku"); 
            }
            
        }

        private void Cnvbutton_Click(object sender, EventArgs e)
        {
            count++;
            label.Text = "";
            string level;
            if (levelBox.SelectedItem == null)
                level = "0";
            else
                level = levelBox.SelectedItem.ToString();

            if (formatBox.Text.ToString() != "")
                Conv(formatBox.SelectedItem.ToString(),
                    textBox.Text.ToString(),
                    level);
            else
                MessageBox.Show("Nie wybrano formatu");
        }

        private void Conv(string format, string filepath, string level)
        {
            Bitmap bmp = new Bitmap(filepath);
            string path = "C:\\Users\\Bartek\\Desktop\\kompresja\\image" + count.ToString() + "." + format.ToLower();

            EncoderParameter enc = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality,100);

            if (Int32.TryParse(level, out int int_level))
               enc = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality,int_level);
            else
                switch (level.ToUpper())
                {
                    case "LZW":
                        enc = new EncoderParameter(System.Drawing.Imaging.Encoder.Compression,
                            (long)EncoderValue.CompressionLZW);
                        break;
                    case "CCITT3":
                        enc = new EncoderParameter(System.Drawing.Imaging.Encoder.Compression,
                            (long)EncoderValue.CompressionCCITT3);
                        break;
                    case "CCITT4":
                        enc = new EncoderParameter(System.Drawing.Imaging.Encoder.Compression,
                            (long)EncoderValue.CompressionCCITT4);
                        break;
                    case "RLE":
                        enc = new EncoderParameter(System.Drawing.Imaging.Encoder.Compression,
                            (long)EncoderValue.CompressionRle);
                        break;
                    case "NONE":
                        enc = new EncoderParameter(System.Drawing.Imaging.Encoder.Compression,
                            (long)EncoderValue.CompressionNone);
                        break;
                }

            EncoderParameters myEncoderParameters = new EncoderParameters(1);
            myEncoderParameters.Param[0] = enc;

            ImageCodecInfo myImageCodecInfo;
            myImageCodecInfo = GetEncoderInfo("image/" + format.ToLower());

            using (Bitmap image = new Bitmap(bmp))
            {
                try
                {
                    switch (format)
                    {
                        case "PNG":
                                image.Save(path, ImageFormat.Png);
                            break;
                        case "BMP":
                            image.Save(path, ImageFormat.Bmp);
                            break;
                        case "JPEG":
                            if (level == "0")
                                image.Save(path, ImageFormat.Jpeg);
                            else
                                image.Save(path, myImageCodecInfo, myEncoderParameters);
                            break;
                        case "TIFF":
                            if (level == "0")
                                image.Save(path, ImageFormat.Tiff);
                            else
                                image.Save(path, myImageCodecInfo, myEncoderParameters);
                            break;
                        case "GIF":
                            image.Save(path, ImageFormat.Gif);
                            break;
                    }
                    label.Text = "Kompresja przebiegła pomyślnie!";
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString());
                }
                Bitmap toDisplayAfterCompress = new Bitmap(path);
                pictureBox.Image = toDisplayAfterCompress;
                
                
            }
        }

        private void FormatBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            levelBox.Items.Clear();
            if (formatBox.Text.ToString() == "JPEG")
                levelBox.Items.AddRange(new object[] { "5", "25", "50", "100" });
            else if (formatBox.Text.ToString() == "TIFF")
                levelBox.Items.AddRange(new object[] { "LZW", "CCITT3", "CCITT4", "Rle", "None" });
            else
                levelBox.Items.Clear();

        }
        private ImageCodecInfo GetEncoderInfo(String mimeType)
        {
            int j;
            ImageCodecInfo[] encoders;
            encoders = ImageCodecInfo.GetImageEncoders();
            for (j = 0; j < encoders.Length; ++j)
            {
                if (encoders[j].MimeType == mimeType)
                    return encoders[j];
            }
            return null;
        }

        private void PictureBox_Click(object sender, EventArgs e)
        {

        }
    }
}
