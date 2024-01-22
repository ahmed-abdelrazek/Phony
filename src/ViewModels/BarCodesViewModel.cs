using BarcodeStandard;
using MahApps.Metro.Controls.Dialogs;
using Microsoft.Win32;
using Phony.Data;
using Phony.Extensions;
using Phony.Models;
using Phony.Views;
using Prism.Commands;
using Prism.Mvvm;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Phony.ViewModels
{
    public class BarcodesViewModel : BindableBase
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
            set => SetProperty(ref _height, value);
        }

        public int Width
        {
            get => _width;
            set => SetProperty(ref _width, value);
        }

        public int BarWidth
        {
            get => _barWidth;
            set => SetProperty(ref _barWidth, value);
        }

        public double AspectRatio
        {
            get => _aspectRatio;
            set => SetProperty(ref _aspectRatio, value);
        }

        public string EncodeValue
        {
            get => _encodeValue;
            set => SetProperty(ref _encodeValue, value);
        }

        public string SelectedRotate
        {
            get => _selectedRotate;
            set => SetProperty(ref _selectedRotate, value);
        }

        public string Alignment
        {
            get => _alignment;
            set => SetProperty(ref _alignment, value);
        }

        public string Foreground
        {
            get => _foreground;
            set => SetProperty(ref _foreground, value);
        }

        public string Background
        {
            get => _background;
            set => SetProperty(ref _background, value);
        }

        public string AlternateLabelText
        {
            get => _alternateLabelText;
            set => SetProperty(ref _alternateLabelText, value);
        }

        public string LabelLocation
        {
            get => _labelLocation;
            set => SetProperty(ref _labelLocation, value);
        }

        public string EncodedValue
        {
            get => _encodedValue;
            set => SetProperty(ref _encodedValue, value);
        }

        public string SelectedEncoder
        {
            get => _selectedEncoder;
            set => SetProperty(ref _selectedEncoder, value);
        }

        public bool GenerateLabel
        {
            get => _generateLabel;
            set => SetProperty(ref _generateLabel, value);
        }

        public byte[] Image
        {
            get => _image;
            set => SetProperty(ref _image, value);
        }

        public List<string> RotateTypes
        {
            get => _rotateTypes;
            set => SetProperty(ref _rotateTypes, value);
        }

        public List<Enumeration<byte>> Encoders
        {
            get => _encoders;
            set => SetProperty(ref _encoders, value);
        }

        private readonly Barcode barCode = new();

        public ICommand SelectForeColor { get; set; }
        public ICommand SelectBackColor { get; set; }
        public ICommand Encode { get; set; }
        public ICommand Save { get; set; }

        readonly BarCodes Message = Application.Current.Windows.OfType<BarCodes>().FirstOrDefault();

        public BarcodesViewModel()
        {
            Foreground = barCode.ForeColor.ToString();
            Background = barCode.BackColor.ToString();
            RotateTypes = [];
            foreach (var item in Enum.GetNames(typeof(RotateFlipType)))
            {
                if (item.ToString().Trim().Equals("rotatenoneflipnone", StringComparison.CurrentCultureIgnoreCase))
                {
                    continue;
                }
                RotateTypes.Add(item.ToString());
            }
            SelectedRotate = "Rotate180FlipXY";
            Encoders = [];
            foreach (var type in Enum.GetValues(typeof(BarCodeEncoders)))
            {
                Encoders.Add(new Enumeration<byte>
                {
                    Id = (byte)type,
                    Name = Enumerations.GetEnumDescription((BarCodeEncoders)type).ToString()
                });
            }
            SelectedEncoder = "Code 128";
            LoadCommands();
        }

        public void LoadCommands()
        {
            SelectForeColor = new DelegateCommand(DoSelectForeColor, CanSelectForeColor);
            SelectBackColor = new DelegateCommand(DoBackColor, CanBackColor);
            Encode = new DelegateCommand(DoEncode, CanEncode).ObservesProperty(() => EncodeValue);
            Save = new DelegateCommand(DoSave, CanSave).ObservesProperty(() => EncodedValue);
        }

        private bool CanSave()
        {
            return !string.IsNullOrWhiteSpace(EncodedValue);
        }

        private void DoSave()
        {
            SaveFileDialog sfd = new()
            {
                Filter = "JPG (*.jpg)|*.jpg|PNG (*.png)|*.png|Webp (*.webp)|*.webp",
                FilterIndex = 2,
                AddExtension = true
            };
            if (sfd.ShowDialog() == true)
            {
                SaveTypes savetype = SaveTypes.Unspecified;
                switch (sfd.FilterIndex)
                {
                    case 1: /* JPG */  savetype = SaveTypes.Jpg; break;
                    case 2: /* PNG */  savetype = SaveTypes.Png; break;
                    case 3: /* Webp */ savetype = SaveTypes.Webp; break;
                    default: break;
                }
                barCode.SaveImage(sfd.FileName, savetype);
            }
        }

        private bool CanEncode()
        {
            return !string.IsNullOrWhiteSpace(EncodeValue) && !string.IsNullOrWhiteSpace(Alignment) && !string.IsNullOrWhiteSpace(SelectedEncoder);
        }

        private async void DoEncode()
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
            barCode.Alignment = Alignment switch
            {
                "يمين" => AlignmentPositions.Right,
                "يسار" => AlignmentPositions.Left,
                _ => AlignmentPositions.Center,
            };
            BarcodeStandard.Type type = BarcodeStandard.Type.Unspecified;
            switch (SelectedEncoder)
            {
                case "UPC-A": type = BarcodeStandard.Type.UpcA; break;
                case "UPC-E": type = BarcodeStandard.Type.UpcE; break;
                case "UPC 2 Digit Ext.": type = BarcodeStandard.Type.UpcSupplemental2Digit; break;
                case "UPC 5 Digit Ext.": type = BarcodeStandard.Type.UpcSupplemental5Digit; break;
                case "EAN-13": type = BarcodeStandard.Type.Ean13; break;
                case "JAN-13": type = BarcodeStandard.Type.Jan13; break;
                case "EAN-8": type = BarcodeStandard.Type.Ean8; break;
                case "ITF-14": type = BarcodeStandard.Type.Itf14; break;
                case "Codabar": type = BarcodeStandard.Type.Codabar; break;
                case "PostNet": type = BarcodeStandard.Type.PostNet; break;
                case "Bookland/ISBN": type = BarcodeStandard.Type.Bookland; break;
                case "Code 11": type = BarcodeStandard.Type.Code11; break;
                case "Code 39": type = BarcodeStandard.Type.Code39; break;
                case "Code 39 Extended": type = BarcodeStandard.Type.Code39Extended; break;
                case "Code 39 Mod 43": type = BarcodeStandard.Type.Code39Mod43; break;
                case "Code 93": type = BarcodeStandard.Type.Code93; break;
                case "LOGMARS": type = BarcodeStandard.Type.Logmars; break;
                case "MSI Mod 10": type = BarcodeStandard.Type.MsiMod10; break;
                case "MSI Mod 11": type = BarcodeStandard.Type.MsiMod11; break;
                case "MSI 2 Mod 10": type = BarcodeStandard.Type.Msi2Mod10; break;
                case "MSI Mod 11 Mod 10": type = BarcodeStandard.Type.MsiMod11Mod10; break;
                case "Interleaved 2 of 5": type = BarcodeStandard.Type.Interleaved2Of5; break;
                case "Interleaved 2 of 5 Mod 10": type = BarcodeStandard.Type.Interleaved2Of5Mod10; break;
                case "Standard 2 of 5": type = BarcodeStandard.Type.Standard2Of5; break;
                case "Standard 2 of 5 Mod 10": type = BarcodeStandard.Type.Standard2Of5Mod10; break;
                case "Code 128": type = BarcodeStandard.Type.Code128; break;
                case "Code 128-A": type = BarcodeStandard.Type.Code128A; break;
                case "Code 128-B": type = BarcodeStandard.Type.Code128B; break;
                case "Code 128-C": type = BarcodeStandard.Type.Code128C; break;
                case "Telepen": type = BarcodeStandard.Type.Telepen; break;
                case "FIM": type = BarcodeStandard.Type.Fim; break;
                case "Pharmacode": type = BarcodeStandard.Type.Pharmacode; break;
            }
            try
            {
                if (type != BarcodeStandard.Type.Unspecified)
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
                    //===== Encoding performed here =====
                    SKColor.TryParse(Foreground, out SKColor sKColorForeground);
                    SKColor.TryParse(Background, out SKColor sKColorBackground);

                    var sKImage = barCode.Encode(type, EncodeValue, sKColorForeground, sKColorBackground, W, H);
                    Image = sKImage.EncodedData.ToArray();
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
            ColorDialog colorDialog = new()
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
            ColorDialog colorDialog = new()
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