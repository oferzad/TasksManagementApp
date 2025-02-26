using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TasksManagementApp.Controlles
{
    public class StarRatingControl : ContentView
    {
        public static readonly BindableProperty RatingProperty =
            BindableProperty.Create(nameof(Rating), typeof(int), typeof(StarRatingControl), 0, BindingMode.TwoWay, propertyChanged: OnRatingChanged);
        public static readonly BindableProperty NumStarsProperty =
            BindableProperty.Create(nameof(NumStars), typeof(int), typeof(StarRatingControl), 0, BindingMode.OneTime, propertyChanged: OnNumStarsChanged);

        public static readonly BindableProperty StarSetColorProperty =
            BindableProperty.Create(nameof(StarSetColor), typeof(Color), typeof(StarRatingControl), Colors.Gold);

        public static readonly BindableProperty StarUnsetColorProperty =
            BindableProperty.Create(nameof(StarUnsetColor), typeof(Color), typeof(StarRatingControl), Colors.Gray);

        private readonly List<ImageButton> _stars;
        private int indexClicked;
        public StarRatingControl()
        {
            _stars = new List<ImageButton>();
            InitContent();
            
        }

        private void InitContent()
        {
            _stars.Clear();
            var stackLayout = new HorizontalStackLayout();

            for (int i = 1; i <= NumStars; i++)
            {
                var starButton = new ImageButton
                {
                    Source = "star.png", // Make sure you have a star image in your resources
                    BackgroundColor = Colors.Transparent,
                    WidthRequest = 40,
                    HeightRequest = 40
                };

                starButton.Clicked += OnStarClicked;


                _stars.Add(starButton);
                stackLayout.Children.Add(starButton);
            }

            Content = stackLayout;
            if (Rating > NumStars)
                    Rating = NumStars;
            UpdateStars();
        }
        public int Rating
        {
            get => (int)GetValue(RatingProperty);
            set
            {
                if (value < 0)
                    SetValue(RatingProperty, 0);
                else if (value > NumStars)
                    SetValue(RatingProperty, NumStars);
                else 
                    SetValue(RatingProperty, value);
            }
        }

        public int NumStars
        {
            get => (int)GetValue(NumStarsProperty);
            set
            {
                if (value < Rating)
                    Rating = value;
                SetValue(NumStarsProperty, value);
            }
        }

        public Color StarSetColor
        {
            get => (Color)GetValue(StarSetColorProperty);
            set => SetValue(StarSetColorProperty, value);
        }

        public Color StarUnsetColor
        {
            get => (Color)GetValue(StarUnsetColorProperty);
            set => SetValue(StarUnsetColorProperty, value);
        }

        private static void OnRatingChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is StarRatingControl control)
            {
                control.UpdateStars();
            }
        }

        private static void OnNumStarsChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is StarRatingControl control)
            {
                control.InitContent();
            }
            
        }

        private void OnStarClicked(Object? source, EventArgs e)
        {
            for(int i=0;i< _stars.Count; i++) {
                if (_stars[i] == source)
                {
                    if (i == 0 && Rating == 1)
                        indexClicked = 0;
                    else
                        indexClicked = i + 1;
                    break;
                }
            }
            Rating = indexClicked;
            UpdateStars();
        }

        private void UpdateStars()
        {
            if (Rating > NumStars)
                Rating = NumStars;
            if (Rating < 0) Rating = 0;
            else if (Rating < 0)
                Rating = 0;
            for (int i = 0; i < _stars.Count; i++)
            {
                if (i < Rating)
                {
                    //_stars[i].BackgroundColor = StarSetColor;
                    _stars[i].Source = "star2.png";
                }
                else
                {
                    //_stars[i].BackgroundColor = StarUnsetColor;
                    _stars[i].Source = "star.png";
                }
            }
        }
    }
}
