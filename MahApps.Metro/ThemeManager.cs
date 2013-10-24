﻿using MahApps.Metro.Controls;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace MahApps.Metro
{
    public static class ThemeManager
    {
        private static readonly ResourceDictionary LightResource = new ResourceDictionary { Source = new Uri("pack://application:,,,/MahApps.Metro;component/Styles/Accents/BaseLight.xaml") };
        private static readonly ResourceDictionary DarkResource = new ResourceDictionary { Source = new Uri("pack://application:,,,/MahApps.Metro;component/Styles/Accents/BaseDark.xaml") };

        private static List<ResourceDictionary> _mainResourceDictionaries;
        public static List<ResourceDictionary> MainResourceDictionaries
        {
            get
            {
                return _mainResourceDictionaries ?? (_mainResourceDictionaries =
                    new List<ResourceDictionary> {
                                                    new ResourceDictionary { Source = new Uri("pack://application:,,,/MahApps.Metro;component/Styles/Colors.xaml") },
                                                    new ResourceDictionary { Source = new Uri("pack://application:,,,/MahApps.Metro;component/Styles/Fonts.xaml") },
                                                    new ResourceDictionary { Source = new Uri("pack://application:,,,/MahApps.Metro;component/Styles/Controls.xaml") },
                                                    new ResourceDictionary { Source = new Uri("pack://application:,,,/MahApps.Metro;component/Styles/Controls.AnimatedSingleRowTabControl.xaml") },
                                                    new ResourceDictionary { Source = new Uri("pack://application:,,,/MahApps.Metro;component/Styles/FlatButton.xaml") }
                                                });
            }
        }

        private static IList<Accent> _accents;
        public static IList<Accent> DefaultAccents
        {
            get
            {
                return _accents ?? (_accents =
                    new List<Accent>{
                        new Accent("Red", new Uri("pack://application:,,,/MahApps.Metro;component/Styles/Accents/Red.xaml")),
                        new Accent("Green", new Uri("pack://application:,,,/MahApps.Metro;component/Styles/Accents/Green.xaml")),
                        new Accent("Blue", new Uri("pack://application:,,,/MahApps.Metro;component/Styles/Accents/Blue.xaml")),
                        new Accent("Purple", new Uri("pack://application:,,,/MahApps.Metro;component/Styles/Accents/Purple.xaml")),
                        new Accent("Orange", new Uri("pack://application:,,,/MahApps.Metro;component/Styles/Accents/Orange.xaml")),

                        new Accent("Lime", new Uri("pack://application:,,,/MahApps.Metro;component/Styles/Accents/Lime.xaml")),
                        new Accent("Emerald", new Uri("pack://application:,,,/MahApps.Metro;component/Styles/Accents/Emerald.xaml")),
                        new Accent("Teal", new Uri("pack://application:,,,/MahApps.Metro;component/Styles/Accents/Teal.xaml")),
                        new Accent("Cyan", new Uri("pack://application:,,,/MahApps.Metro;component/Styles/Accents/Cyan.xaml")),
                        new Accent("Cobalt", new Uri("pack://application:,,,/MahApps.Metro;component/Styles/Accents/Cobalt.xaml")),
                        new Accent("Indigo", new Uri("pack://application:,,,/MahApps.Metro;component/Styles/Accents/Indigo.xaml")),
                        new Accent("Violet", new Uri("pack://application:,,,/MahApps.Metro;component/Styles/Accents/Violet.xaml")),
                        new Accent("Pink", new Uri("pack://application:,,,/MahApps.Metro;component/Styles/Accents/Pink.xaml")),
                        new Accent("Magenta", new Uri("pack://application:,,,/MahApps.Metro;component/Styles/Accents/Magenta.xaml")),
                        new Accent("Crimson", new Uri("pack://application:,,,/MahApps.Metro;component/Styles/Accents/Crimson.xaml")),
                        new Accent("Amber", new Uri("pack://application:,,,/MahApps.Metro;component/Styles/Accents/Amber.xaml")),
                        new Accent("Yellow", new Uri("pack://application:,,,/MahApps.Metro;component/Styles/Accents/Yellow.xaml")),
                        new Accent("Brown", new Uri("pack://application:,,,/MahApps.Metro;component/Styles/Accents/Brown.xaml")),
                        new Accent("Olive", new Uri("pack://application:,,,/MahApps.Metro;component/Styles/Accents/Olive.xaml")),
                        new Accent("Steel", new Uri("pack://application:,,,/MahApps.Metro;component/Styles/Accents/Steel.xaml")),
                        new Accent("Mauve", new Uri("pack://application:,,,/MahApps.Metro;component/Styles/Accents/Mauve.xaml")),
                        new Accent("Sienna", new Uri("pack://application:,,,/MahApps.Metro;component/Styles/Accents/Sienna.xaml")),
                    });
            }
        }

        public static void ChangeTheme(Application app, Accent accent, Theme theme)
        {
            ChangeTheme(app.Resources, accent, theme);
        }

        public static void ChangeTheme(Window window, Accent accent, Theme theme)
        {
            window.Resources.BeginInit();

            var detectedTheme = DetectTheme((MetroWindow)window);
            if (detectedTheme != null)
            {
                if (detectedTheme.Item2 != null)
                {
                    var accentResource = window.Resources.MergedDictionaries.FirstOrDefault(d => d.Source == detectedTheme.Item2.Resources.Source);
                    if (accentResource != null) {
                        var ok = window.Resources.MergedDictionaries.Remove(accentResource);

                        foreach (DictionaryEntry r in accentResource)
                        {
                            if (window.Resources.Contains(r.Key))
                                window.Resources.Remove(r.Key);
                        }

                        window.Resources.MergedDictionaries.Add(accent.Resources);
                    }
                }
                if (detectedTheme.Item1 != null)
                {
                    var themeResource = (detectedTheme.Item1 == Theme.Light) ? LightResource : DarkResource;
                    var md = window.Resources.MergedDictionaries.FirstOrDefault(d => d.Source == themeResource.Source);
                    if (md != null)
                    {
                        window.Resources.MergedDictionaries.Remove(md);
                        var newThemeResource = (theme == Theme.Light) ? LightResource : DarkResource;

                        foreach (DictionaryEntry r in themeResource)
                        {
                            if (window.Resources.Contains(r.Key))
                                window.Resources.Remove(r.Key);
                        }

                        window.Resources.MergedDictionaries.Add(newThemeResource);
                    }
                }
            }
            else
            {
                ChangeTheme(window.Resources, accent, theme);
            }

            window.Resources.EndInit();
        }

        public static void ChangeTheme(ResourceDictionary r, Accent accent, Theme theme)
        {
            var themeResource = (theme == Theme.Light) ? LightResource : DarkResource;
            ApplyResourceDictionary(accent.Resources, r);
            ApplyResourceDictionary(themeResource, r);
        }

        private static void ApplyResourceDictionary(ResourceDictionary newRd, ResourceDictionary oldRd)
        {
            foreach (DictionaryEntry r in newRd)
            {
                if (oldRd.Contains(r.Key))
                    oldRd.Remove(r.Key);

                oldRd.Add(r.Key, r.Value);
            }
        }

        /// <summary>
        /// Scans a Window's resources and returns it's accent and theme.
        /// </summary>
        /// <param name="window">The Window to check.</param>
        /// <returns></returns>
        public static Tuple<Theme, Accent> DetectTheme(MahApps.Metro.Controls.MetroWindow window)
        {
            if (window == null) throw new ArgumentNullException("window");

            Theme currentTheme = Theme.Light;
            ResourceDictionary themeDictionary = null;
            Tuple<Theme, Accent> detectedAccentTheme = null;


            if (DetectThemeFromResources(ref currentTheme, ref themeDictionary, window.Resources))
            {
                if (GetThemeFromResources(currentTheme, window.Resources, ref detectedAccentTheme))
                    return new Tuple<Theme, Accent>(detectedAccentTheme.Item1, detectedAccentTheme.Item2);
            }

            return null;
        }

        public static Tuple<Theme, Accent> DetectTheme(Application app)
        {
            if (app == null) throw new ArgumentNullException("app");

            Theme currentTheme = Theme.Light;
            ResourceDictionary themeDictionary = null;
            Tuple<Theme, Accent> detectedAccentTheme = null;


            if (DetectThemeFromResources(ref currentTheme, ref themeDictionary, app.Resources)) {
                if (GetThemeFromResources(currentTheme, app.Resources, ref detectedAccentTheme))
                    return new Tuple<Theme, Accent>(detectedAccentTheme.Item1, detectedAccentTheme.Item2);
            }

            return null;
        }

        internal static bool DetectThemeFromAppResources(out Theme detectedTheme, out ResourceDictionary themeRd)
        {
            detectedTheme = Theme.Light;
            themeRd = null;

            return DetectThemeFromResources(ref detectedTheme, ref themeRd, Application.Current.Resources);
        }

        private static bool DetectThemeFromResources(ref Theme detectedTheme, ref ResourceDictionary themeRd, ResourceDictionary dict)
        {
            var enumerator = dict.MergedDictionaries.GetEnumerator();
            while (enumerator.MoveNext())
            {
                var currentRd = enumerator.Current;

                if (currentRd.Source == LightResource.Source || currentRd.Source == DarkResource.Source)
                {
                    detectedTheme = currentRd.Source == LightResource.Source ? Theme.Light : Theme.Dark;
                    themeRd = currentRd;

                    enumerator.Dispose();
                    return true;
                }

                if (DetectThemeFromResources(ref detectedTheme, ref themeRd, currentRd)) {
                    return true;
                }
            }

            enumerator.Dispose();
            return false;
        }
        
        internal static bool GetThemeFromResources(Theme presetTheme, ResourceDictionary dict, ref Tuple<Theme, Accent> detectedAccentTheme)
        {
            Theme currentTheme = presetTheme;
            Accent currentAccent = null;

            Accent matched = null;
            if ((matched = ((List<Accent>)DefaultAccents).Find(x => x.Resources.Source == dict.Source)) != null)
            {
                currentAccent = matched;
                detectedAccentTheme = Tuple.Create<Theme, Accent>(currentTheme, currentAccent);
                return true;
            }

            foreach (ResourceDictionary rd in dict.MergedDictionaries)
            {
                if (GetThemeFromResources(presetTheme, rd, ref detectedAccentTheme))
                    return true;
            }

            return false;
        }
    }
}