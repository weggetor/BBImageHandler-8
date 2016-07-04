/* 
  THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF 
  ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO 
  THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A 
  PARTICULAR PURPOSE. 
  
    This is sample code and is freely distributable. 
*/

using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace Bitboxx.Web.GeneratedImage.Transform
{
	public class IEBrowser : ApplicationContext
	{
		public Image Thumb; 
		private string _html;
		private UrlRatioMode _ratio;
		AutoResetEvent ResultEvent;
	    private WebBrowser _browser;

        public enum UrlRatioMode
        {
            Full,
            Screen,
            Cinema
        }

        public IEBrowser(string target,  UrlRatioMode ratio, AutoResetEvent resultEvent)
		{

            ResultEvent = resultEvent;
			Thread thrd = new Thread(new ThreadStart(
			                         	delegate {
			                         	         	Init(target,ratio);
			                         	         	Application.Run(this);
			                         	})); 
			// set thread to STA state before starting
			thrd.SetApartmentState(ApartmentState.STA);
			thrd.Start(); 
		}

		private void Init(string target,UrlRatioMode ratio)
		{
			// create a WebBrowser control
			_browser = new WebBrowser();
            _browser.ScrollBarsEnabled = false;
            _browser.ScriptErrorsSuppressed = true;

            // set WebBrowser event handle
            _browser.DocumentCompleted += IEBrowser_DocumentCompleted;

			_ratio = ratio;

			if (target.ToLower().StartsWith("http:")|| target.ToLower().StartsWith("https:"))
			{
				_html = "";
                _browser.Navigate(target);
			}
			else
			{
                _browser.Navigate("about:blank");
				_html = target;
			}
		}

		// DocumentCompleted event handle
		void IEBrowser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
		{
            string url = e.Url.ToString();
            if (!(url.StartsWith("http://") || url.StartsWith("https://")))
            {
                // in AJAX
            }

            if (e.Url.AbsolutePath != _browser.Url.AbsolutePath)
            {
                // IFRAME 
            }
            else
            {
                // REAL DOCUMENT COMPLETE
                try
                {
                    WebBrowser browser = (WebBrowser)sender;
                    browser.Width = 1280;
                    browser.Height = 1024;

                    HtmlDocument doc = browser.Document;
                    if (_html != String.Empty)
                    {
                        doc.OpenNew(true);
                        doc.Write(_html);
                    }
                    switch (_ratio)
                    {
                        case UrlRatioMode.Full:
                            browser.Width = doc.Body.ScrollRectangle.Width;
                            browser.Height = doc.Body.ScrollRectangle.Height;
                            break;
                        case UrlRatioMode.Screen:
                            browser.Width = doc.Body.ScrollRectangle.Width;
                            browser.Height = Convert.ToInt32(browser.Width / 3 * 2);
                            break;
                        case UrlRatioMode.Cinema:
                            browser.Width = doc.Body.ScrollRectangle.Width;
                            browser.Height = Convert.ToInt32(browser.Width / 16 * 9);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                    Bitmap bitmap = new Bitmap(browser.Width, browser.Height);
                    GetImage(browser.ActiveXInstance, bitmap, Color.White);

                    browser.Dispose();
                    Thumb = (Image)bitmap;
                }
                catch (Exception)
                {
                }
                finally
                {
                    ResultEvent.Set();
                }
            }
            

		}

		[ComImport]
		[Guid("0000010D-0000-0000-C000-000000000046")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		private interface IViewObject
		{
			void Draw([MarshalAs(UnmanagedType.U4)] uint dwAspect, int lindex, IntPtr pvAspect, [In] IntPtr ptd, IntPtr hdcTargetDev, IntPtr hdcDraw, [MarshalAs(UnmanagedType.Struct)] ref RECT lprcBounds, [In] IntPtr lprcWBounds, IntPtr pfnContinue, [MarshalAs(UnmanagedType.U4)] uint dwContinue);
		}

		[StructLayout(LayoutKind.Sequential, Pack = 4)]
		private struct RECT
		{
			public int Left;
			public int Top;
			public int Right;
			public int Bottom;
		}

		public static void GetImage(object obj, Image destination, Color backgroundColor)
		{
			using (Graphics graphics = Graphics.FromImage(destination))
			{
				IntPtr deviceContextHandle = IntPtr.Zero;
				RECT rectangle = new RECT();

				rectangle.Right = destination.Width;
				rectangle.Bottom = destination.Height;

				graphics.Clear(backgroundColor);

				try
				{
					deviceContextHandle = graphics.GetHdc();

					IViewObject viewObject = obj as IViewObject;
					viewObject.Draw(1, -1, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, deviceContextHandle, ref rectangle, IntPtr.Zero, IntPtr.Zero, 0);
				}
				finally
				{
					if (deviceContextHandle != IntPtr.Zero)
					{
						graphics.ReleaseHdc(deviceContextHandle);
					}
				}
			}
		}
	}
}