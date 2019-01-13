﻿using System.IO;
using System.Reflection;
using Naxam.Controls.Forms;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using System.Linq;
using SVG.Forms.Plugin.Abstractions;
using LabelHtml.Forms.Plugin.Abstractions;

namespace InterfaceBuilder
{
    public class UI
    {
        public Theme Theme { get; private set; }

        public UI(Theme theme = null) => this.Theme = theme ?? new Theme();

        public Label Label(string text = "")
        {
            return new Label {
                HorizontalTextAlignment = TextAlignment.Start,
                TextColor = Theme.Colors.Primary.Foreground,
                FontSize = Theme.Sizes.NormalFont,
                VerticalTextAlignment = TextAlignment.Center,
                Margin = new Thickness(0, Theme.Sizes.NormalFont / 2),
                LineBreakMode = LineBreakMode.WordWrap,
            }.Text(text);
        }

        public Frame Button(string text = "")
        {
            return Frame(
                Label(text).Color(Theme.Colors.Accent.Foreground)
            ).NoBorder().Padding(Theme.Sizes.NormalFont / 2).Background(Theme.Colors.Accent.Background);
        }

        public Image Image(string resourceId = "")
        {
            var image = new Image {
                Source = ImageSource.FromResource(resourceId, FindCallingAssembly()),
                Aspect = Aspect.AspectFill,
            };
            image.Source.AutomationId = resourceId;
            return image;
        }

        public HtmlLabel Html(string content)
        {
            var ressource = FindCallingAssembly().GetManifestResourceStream(content);

            if (ressource != null)
                content = new StreamReader(ressource).ReadToEnd();


            return new HtmlLabel {
                Text = content,
            }.FontSize(Theme.Sizes.NormalFont);
        }

        public WebView Website(string url)
        {
            return new WebView {
                Source = url
            };
        }

        public SvgImage Svg(string ressourceId)
        {
            return new SvgImage {
                SvgAssembly = FindCallingAssembly(),
                SvgPath = ressourceId,
                //WidthRequest = 100,
                //HeightRequest = 100,
            };
        }

        public Xamarin.Forms.Entry Entry(string placeholder = "")
        {
            return new Xamarin.Forms.Entry {
                TextColor = Theme.Colors.Primary.Foreground,
                FontSize = Theme.Sizes.NormalFont,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Placeholder = placeholder,
                BackgroundColor = Theme.Colors.Primary.Background,
                Margin = new Thickness(0, Theme.Sizes.NormalFont / 2.0),
            };
        }

        public Label Headline(string text = "")
        {
            var headline = Label(text).Bold().FontSize(Theme.Sizes.HeadlineFont).
                      Margin(0, 2 * Theme.Sizes.NormalFont, 0, Theme.Sizes.NormalFont);

            // NOTE headlines have a big top margin unless they are the first item (eg. beginning of a page)s
            headline.MeasureInvalidated += (sender, e) => {
                if (headline.Parent is Layout layout)
                    if (layout.Children.FirstOrDefault() == headline)
                        headline.Margin(0, 0, 0, Theme.Sizes.NormalFont);
            };

            return headline;
        }

        public StackLayout Action(string text = "", string icon = null)
        {
            var stack = Stack().Horizontal().Padding(new Thickness(25, 0));
            if (icon != null)
                stack.Children.Insert(0, Icon(icon).Margin(0, 0, Theme.Sizes.PageMargin, 0));
            return stack.With(Label(text));
        }

        public StackLayout Stack()
        {
            return new StackLayout {
                BackgroundColor = Xamarin.Forms.Color.Transparent,
                Spacing = 0,
            };
        }

        public ActivityIndicator ActivityIndicator()
        {
            return new ActivityIndicator { IsRunning = true, IsVisible = true, Margin = 10 };
        }

        public BoxView Box()
        {
            return new BoxView {
                BackgroundColor = Xamarin.Forms.Color.Transparent,
            };
        }

        public BoxView Box(int size)
        {
            return new BoxView {
                BackgroundColor = Xamarin.Forms.Color.Transparent,
                WidthRequest = size,
                HeightRequest = size,
                Margin = new Thickness(0, Theme.Sizes.NormalFont / 2.0),
            };
        }

        public Frame Frame(View content)
        {
            return new Frame {
                BackgroundColor = Theme.Colors.Primary.Background,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Content = content,
                BorderColor = Theme.Colors.Primary.Background.WithLuminosity(0.5),
                HasShadow = false,
                CornerRadius = 0
            }.Padding(5);
        }

        public Xamarin.Forms.ScrollView Scrollable(View content)
        {
            return new Xamarin.Forms.ScrollView {
                Content = content
            };
        }

        public FlexLayout Flex()
        {
            return new FlexLayout() {
                AlignItems = FlexAlignItems.Center,
                JustifyContent = FlexJustify.SpaceBetween,
                AlignContent = FlexAlignContent.SpaceEvenly,
                HorizontalOptions = LayoutOptions.FillAndExpand,
            };
        }

        public MasterDetailPage MasterDetailPage(ContentPage menu, Xamarin.Forms.NavigationPage initialContent)
        {
            return new MasterDetailPage {
                Master = menu,
                Detail = initialContent
            };
        }

        public ContentPage Page(string title, View content)
        {
            var page = new ContentPage {
                Title = title,
                Content = content,
                BackgroundColor = Theme.Colors.Primary.Background,
                Padding = Theme.Sizes.PageMargin,
            };

            return page;
        }

        public ContentPage ScrollPage(string title, View content)
        {
            return Page(title, Scrollable(content));
        }

        public TabbedPage TabbedPage(string title, params Xamarin.Forms.Page[] pages)
        {
            var page = new TopTabbedPage {
                Title = title,
                BarTextColor = Theme.Colors.Accent.Background,
                BarIndicatorColor = Theme.Colors.Accent.Background,
                BarBackgroundColor = Theme.Colors.Accent.Background,
            };

            pages.ForEach(p => page.Children.Add(p));

            return page.WithSafeAreas();
        }

        public Xamarin.Forms.NavigationPage NavigationPage(string title, View content)
        {
            return NavigationPage(Page(title, content)).WithSafeAreas();
        }

        public Xamarin.Forms.NavigationPage NavigationPage(Xamarin.Forms.Page page)
        {
            var navigation = new Xamarin.Forms.NavigationPage(page);
            navigation.BarBackgroundColor = Theme.Colors.Accent.Background;
            navigation.BarTextColor = Theme.Colors.Accent.Foreground;
            return navigation.WithSafeAreas();
        }

        public BoxView HorizontalFill()
        {
            return new BoxView() {
                HorizontalOptions = LayoutOptions.FillAndExpand,
            };
        }

        public BoxView VerticalFill()
        {
            return new BoxView() {
                VerticalOptions = LayoutOptions.FillAndExpand,
            };
        }

        public Label Icon(string icon)
        {
            var fontFamily = "";
            if (Device.RuntimePlatform == Device.Android)
                fontFamily = "Elusive-Icons.ttf#Elusive-Icons Regular";
            else if (Device.RuntimePlatform == Device.iOS)
                fontFamily = "elusiveicons";

            return new Label {
                Text = icon,
                VerticalTextAlignment = TextAlignment.Center,
                HorizontalTextAlignment = TextAlignment.Center,
                MinimumWidthRequest = 1e6, // https://stackoverflow.com/questions/41861020/
                TextColor = Theme.Colors.Primary.Foreground,
                FontFamily = fontFamily,
                FontSize = 25,
            };
        }

        public Xamarin.Forms.Slider Slider(double min, double max)
        {
            return new Xamarin.Forms.Slider {
                Maximum = max, // Set max before min to avoid crash
                Minimum = min,
            };
        }

        static Assembly FindCallingAssembly()
        {
            var stackTrace = new System.Diagnostics.StackTrace();
            var frame = stackTrace.GetFrames()[2]; // NOTE assuming that this helper method will be called from UI methods which get called from production code where the ressource is defined
            var method = frame.GetMethod();
            string methodName = method.Name;
            var methodsClass = method.DeclaringType;
            var assembly = methodsClass.Assembly;
            return assembly;
        }
    }
}