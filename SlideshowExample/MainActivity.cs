using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Support.V4.View;
using Android.Support.V4.App;

using FragmentManager = Android.Support.V4.App.FragmentManager;
using Fragment = Android.Support.V4.App.Fragment;
using System.Net;
using Android.Graphics;


namespace SlideshowExample
{
    [Activity(Label = "SlideshowExample", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : FragmentActivity
    {
        private ViewPager _imagePager;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Main);
            _imagePager = FindViewById<ViewPager>(Resource.Id.imagePager);
            _imagePager.Adapter = new ImageFragementAdapter(SupportFragmentManager);
        }


    }

    public class ImageFragementAdapter : FragmentPagerAdapter
    {

        //swich to FragemntStatePagerAdapter
        public ImageFragementAdapter(FragmentManager fm)
            : base(fm)
        {

        }


        public override Fragment GetItem(int position)
        {
            return new ImageFragment(position);
        }

        public override int Count
        {
            get { return 7; }
        }

    }
    public class ImageFragment : Fragment
    {
        string[] urls = new string[] { "http://www.lolcats.com/images/u/11/23/lolcatsdotcomuu378xml5m6xkh1c.jpg", 
        "http://www.lolcats.com/images/u/08/35/lolcatsdotcomaxdjl1t6rivbjr5u.jpg", 
        "http://www.lolcats.com/images/u/12/24/lolcatsdotcomseriously.jpg", 
        "http://www.lolcats.com/images/u/08/32/lolcatsdotcombkf8azsotkiwu8z2.jpg",
        "http://www.lolcats.com/images/u/09/03/lolcatsdotcomqicuw9j0uqt8a23t.jpg", 
        "http://www.lolcats.com/images/u/12/24/lolcatsdotcompromdate.jpg", 
        "http://3.bp.blogspot.com/-iWZ2WzwPr7E/TciqLegMuHI/AAAAAAAAAD0/bAuHtfDO-5w/s1600/lolcats.jpg" };

        private ImageView _imageView;
        private int _position;
        public ImageFragment() { }
        public ImageFragment(int position)
        {
            _position = position;
            RetainInstance = true;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.ImageFragment, container, false);
            view.Id = _position;

            _imageView = view.FindViewById<ImageView>(Resource.Id.imageView);
            GetImageBitmapFromUrlAsync(urls[_position]);
            TextView tv = view.FindViewById<TextView>(Resource.Id.txtView);
            tv.Text = "View " + _position.ToString();

            return view;
        }

        private void GetImageBitmapFromUrlAsync(string url)
        {

            WebClient webClient = new WebClient();
            webClient.DownloadDataCompleted += delegate(object sender, DownloadDataCompletedEventArgs e)
            {
                if (e.Result != null && e.Result.Length > 0)
                {
                    var options = new BitmapFactory.Options
                    {
                        InJustDecodeBounds = false,
                    };
                    // BitmapFactory.DecodeResource() will return a non-null value; dispose of it.
                    using (var dispose = BitmapFactory.DecodeByteArray(e.Result, 0, e.Result.Length, options))
                        _imageView.SetImageBitmap(dispose);
                }
            };
            webClient.DownloadDataAsync(new Uri(url));

        }


    }
}

