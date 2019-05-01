using Caliburn.Micro;
using MahApps.Metro.Controls.Dialogs;
using Microsoft.Win32;
using Phony.WPF.Data;
using Phony.WPF.Extensions;
using Phony.WPF.Models;
using Phony.WPF.Views;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Phony.WPF.ViewModels
{
    public class BarcodesViewModel : Screen
    {
        int _height;
        int _width;
        int _barWidth;
        double _aspectRatio;
        string _encodeValue;
        string _selectedRotate;
        string _alignment;
        string _foreground;
        string _background;
        string _alternateLabelText;
        string _labelLocation;
        string _encodedValue;
        string _selectedEncoder;
        byte[] _image;
        bool _generateLabel;

        List<string> _rotateTypes;
        List<Enumeration<byte>> _encoders;

        public int Height
        {
            get => _height;
            set
            {
                _height = value;
                NotifyOfPropertyChange(() => Height);
            }
        }

        public int Width
        {
            get => _width;
            set
            {
                _width = value;
                NotifyOfPropertyChange(() => Width);
            }
        }

        public int BarWidth
        {
            get => _barWidth;
            set
            {
                _barWidth = value;
                NotifyOfPropertyChange(() => BarWidth);
            }
        }

        public double AspectRatio
        {
            get => _aspectRatio;
            set
            {
                _aspectRatio = value;
                NotifyOfPropertyChange(() => AspectRatio);
            }
        }

        public string EncodeValue
        {
            get => _encodeValue;
            set
            {
                _encodeValue = value;
                NotifyOfPropertyChange(() => EncodeValue);
            }
        }

        public string SelectedRotate
        {
            get => _selectedRotate;
            set
            {
                _selectedRotate = value;
                NotifyOfPropertyChange(() => SelectedRotate);
            }
        }

        public string Alignment
        {
            get => _alignment;
            set
            {
                _alignment = value;
                NotifyOfPropertyChange(() => Alignment);
            }
        }

        public string Foreground
        {
            get => _foreground;
            set
            {
                _foreground = value;
                NotifyOfPropertyChange(() => Foreground);
            }
        }

        public string Background
        {
            get => _background;
            set
            {
                _background = value;
                NotifyOfPropertyChange(() => Background);
            }
        }

        public string AlternateLabelText
        {
            get => _alternateLabelText;
            set
            {
                _alternateLabelText = value;
                NotifyOfPropertyChange(() => AlternateLabelText);
            }
        }

        public string LabelLocation
        {
            get => _labelLocation;
            set
            {
                _labelLocation = value;
                NotifyOfPropertyChange(() => LabelLocation);
            }
        }

        public string EncodedValue
        {
            get => _encodedValue;
            set
            {
                _encodedValue = value;
                NotifyOfPropertyChange(() => EncodedValue);
                NotifyOfPropertyChange(() => CanEncode());
                NotifyOfPropertyChange(() => CanSave());
            }
        }

        public string SelectedEncoder
        {
            get => _selectedEncoder;
            set
            {
                _selectedEncoder = value;
                NotifyOfPropertyChange(() => SelectedEncoder);
            }
        }

        public bool GenerateLabel
        {
            get => _generateLabel;
            set
            {
                _generateLabel = value;
                NotifyOfPropertyChange(() => GenerateLabel);
            }
        }

        public byte[] Image
        {
            get => _image;
            set
            {
                _image = value;
                NotifyOfPropertyChange(() => Image);
            }
        }

        public List<string> RotateTypes
        {
            get => _rotateTypes;
            set
            {
                _rotateTypes = value;
                NotifyOfPropertyChange(() => RotateTypes);
            }
        }

        public List<Enumeration<byte>> Encoders
        {
            get => _encoders;
            set
            {
                _encoders = value;
                NotifyOfPropertyChange(() => Encoders);
            }
        }

        BarcodeLib.Barcode barCode = new BarcodeLib.Barcode();

        readonly BarCodes Message = Application.Current.Windows.OfType<BarCodes>().FirstOrDefault();

        public BarcodesViewModel()
        {
            Foreground = barCode.ForeColor.ToHexString();
            Background = barCode.BackColor.ToHexString();
            RotateTypes = new List<string>();
            foreach (var item in Enum.GetNames(typeof(RotateFlipType)))
            {
                if (item.ToString().Trim().ToLower() == "rotatenoneflipnone")
                {
                    continue;
                }
                RotateTypes.Add(item.ToString());
            }
            SelectedRotate = "Rotate180FlipXY";
            Encoders = new List<Enumeration<byte>>();
            foreach (var type in Enum.GetValues(typeof(BarCodeEncoders)))
            {
                Encoders.Add(new Enumeration<byte>
                {
                    Id = (byte)type,
                    Name = Enumerations.GetEnumDescription((BarCodeEncoders)type).ToString()
                });
            }
            SelectedEncoder = "Code 128";
        }

        private bool CanSave()
        {
            return !string.IsNullOrWhiteSpace(EncodedValue);
        }

        private void Save()
        {
            SaveFileDialog sfd = new SaveFileDialog
            {
                Filter = "BMP (*.bmp)|*.bmp|GIF (*.gif)|*.gif|JPG (*.jpg)|*.jpg|PNG (*.png)|*.png|TIFF (*.tif)|*.tif",
                FilterIndex = 2,
                AddExtension = true
            };
            if (sfd.ShowDialog() == true)
            {
                BarcodeLib.SaveTypes savetype = BarcodeLib.SaveTypes.UNSPECIFIED;
                switch (sfd.FilterIndex)
                {
                    case 1: /* BMP */  savetype = BarcodeLib.SaveTypes.BMP; break;
                    case 2: /* GIF */  savetype = BarcodeLib.SaveTypes.GIF; break;
                    case 3: /* JPG */  savetype = BarcodeLib.SaveTypes.JPG; break;
                    case 4: /* PNG */  savetype = BarcodeLib.SaveTypes.PNG; break;
                    case 5: /* TIFF */ savetype = BarcodeLib.SaveTypes.TIFF; break;
                    default: break;
                }
                barCode.SaveImage(sfd.FileName, savetype);
            }
        }

        private bool CanEncode()
        {
            return !string.IsNullOrWhiteSpace(EncodeValue) && !string.IsNullOrWhiteSpace(Alignment) && !string.IsNullOrWhiteSpace(SelectedEncoder);
        }

        private async void Encode()
        {
            int W = 202;
            int H = 101;
            if (Width > 0)
            {
                W = Width;
            }
            if (Height > 0)
            {
                H = Height;
            }
            switch (Alignment)
            {
                case "يمين": barCode.Alignment = BarcodeLib.AlignmentPositions.RIGHT; break;
                case "يسار": barCode.Alignment = BarcodeLib.AlignmentPositions.LEFT; break;
                default: barCode.Alignment = BarcodeLib.AlignmentPositions.CENTER; break;
            }
            BarcodeLib.TYPE type = BarcodeLib.TYPE.UNSPECIFIED;
            switch (SelectedEncoder)
            {
                case "UPC-A": type = BarcodeLib.TYPE.UPCA; break;
                case "UPC-E": type = BarcodeLib.TYPE.UPCE; break;
                case "UPC 2 Digit Ext.": type = BarcodeLib.TYPE.UPC_SUPPLEMENTAL_2DIGIT; break;
                case "UPC 5 Digit Ext.": type = BarcodeLib.TYPE.UPC_SUPPLEMENTAL_5DIGIT; break;
                case "EAN-13": type = BarcodeLib.TYPE.EAN13; break;
                case "JAN-13": type = BarcodeLib.TYPE.JAN13; break;
                case "EAN-8": type = BarcodeLib.TYPE.EAN8; break;
                case "ITF-14": type = BarcodeLib.TYPE.ITF14; break;
                case "Codabar": type = BarcodeLib.TYPE.Codabar; break;
                case "PostNet": type = BarcodeLib.TYPE.PostNet; break;
                case "Bookland/ISBN": type = BarcodeLib.TYPE.BOOKLAND; break;
                case "Code 11": type = BarcodeLib.TYPE.CODE11; break;
                case "Code 39": type = BarcodeLib.TYPE.CODE39; break;
                case "Code 39 Extended": type = BarcodeLib.TYPE.CODE39Extended; break;
                case "Code 39 Mod 43": type = BarcodeLib.TYPE.CODE39_Mod43; break;
                case "Code 93": type = BarcodeLib.TYPE.CODE93; break;
                case "LOGMARS": type = BarcodeLib.TYPE.LOGMARS; break;
                case "MSI": type = BarcodeLib.TYPE.MSI_Mod10; break;
                case "Interleaved 2 of 5": type = BarcodeLib.TYPE.Interleaved2of5; break;
                case "Standard 2 of 5": type = BarcodeLib.TYPE.Standard2of5; break;
                case "Code 128": type = BarcodeLib.TYPE.CODE128; break;
                case "Code 128-A": type = BarcodeLib.TYPE.CODE128A; break;
                case "Code 128-B": type = BarcodeLib.TYPE.CODE128B; break;
                case "Code 128-C": type = BarcodeLib.TYPE.CODE128C; break;
                case "Telepen": type = BarcodeLib.TYPE.TELEPEN; break;
                case "FIM": type = BarcodeLib.TYPE.FIM; break;
                case "Pharmacode": type = BarcodeLib.TYPE.PHARMACODE; break;
            }
            try
            {
                if (type != BarcodeLib.TYPE.UNSPECIFIED)
                {
                    try
                    {
                        barCode.BarWidth = BarWidth < 1 ? 2 : BarWidth;
                    }
                    catch (Exception ex)
                    {
                        Core.SaveException(ex);
                        await Message.ShowMessageAsync("مشكلة", "هناك مشكله تخص عرض الكود");
                    }
                    try
                    {
                        barCode.AspectRatio = AspectRatio < 1 ? null : (double?)AspectRatio;
                    }
                    catch (Exception ex)
                    {
                        Core.SaveException(ex);
                        await Message.ShowMessageAsync("مشكلة ", "هناك مشكله تخص النسبة");
                    }
                    barCode.IncludeLabel = GenerateLabel;
                    barCode.RotateFlipType = (RotateFlipType)Enum.Parse(typeof(RotateFlipType), SelectedRotate, true);

                    barCode.AlternateLabel = !String.IsNullOrEmpty(AlternateLabelText) ? AlternateLabelText : EncodeValue;
                    switch (LabelLocation)
                    {
                        case "اسفل - يمين": barCode.LabelPosition = BarcodeLib.LabelPositions.BOTTOMRIGHT; break;
                        case "اسفل - يسار": barCode.LabelPosition = BarcodeLib.LabelPositions.BOTTOMLEFT; break;
                        case "اعلى - وسط": barCode.LabelPosition = BarcodeLib.LabelPositions.TOPCENTER; break;
                        case "اعلى - يمين": barCode.LabelPosition = BarcodeLib.LabelPositions.TOPLEFT; break;
                        case "اعلى - يسار": barCode.LabelPosition = BarcodeLib.LabelPositions.TOPRIGHT; break;
                        default: barCode.LabelPosition = BarcodeLib.LabelPositions.BOTTOMCENTER; break;
                    }
                    //===== Encoding performed here =====
                    Image = barCode.Encode(type, EncodeValue, ColorTranslator.FromHtml(Foreground), ColorTranslator.FromHtml(Background), W, H).ImageToByteArray();
                    //===================================
                    EncodedValue = barCode.EncodedValue;
                    // Read dynamically calculated Width/Height because the user is interested.
                    if (barCode.BarWidth.HasValue)
                    {
                        Height = barCode.Height;
                        Width = barCode.Width;
                        BarWidth = (int)barCode.BarWidth;
                    }
                    if (barCode.AspectRatio.HasValue)
                    {
                        AspectRatio = (double)barCode.AspectRatio;
                    }
                }
            }
            catch (Exception ex)
            {
                Core.SaveException(ex);
            }
        }

        private bool CanSelectForeColor()
        {
            return true;
        }

        private void DoSelectForeColor()
        {
            ColorDialog colorDialog = new ColorDialog
            {
                SelectedColor = ((SolidColorBrush)(new BrushConverter().ConvertFrom(Foreground))).Color
            };
            if ((bool)colorDialog.ShowDialog())
            {
                Foreground = colorDialog.SelectedColor.ToHexString();
            }
        }

        private bool CanBackColor()
        {
            return true;
        }

        private void DoBackColor()
        {
            ColorDialog colorDialog = new ColorDialog
            {
                SelectedColor = ((SolidColorBrush)(new BrushConverter().ConvertFrom(Background))).Color
            };
            if ((bool)colorDialog.ShowDialog())
            {
                Background = colorDialog.SelectedColor.ToHexString();
            }
        }
    }
}