﻿using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using System.Windows.Shapes;
using Whathecode.System.Arithmetic.Interpolation;
using Whathecode.System.Arithmetic.Interpolation.KeyPoint;
using Whathecode.System.Arithmetic.Interpolation.TypeProvider;
using Whathecode.System.Arithmetic.Interpolation.TypeProvider.Windows;
using Whathecode.System.Arithmetic.Interpolation.TypeProvider.Windows.Media.Media3D;
using Whathecode.System.Collections.Generic;


namespace InterpolationTest
{
	/// <summary>
	///   Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow
	{
		public MainWindow()
		{
			InitializeComponent();

			// Create a set of known points.
			var keyPoints = new CumulativeKeyPointCollection<Point, double>(
				new PointInterpolationProvider()
				)
			{
				new Point( 50, 50 ),
				new Point( 100, 100 ),
				new Point( 100, 200 ),
				new Point( 300, 20 ),
				new Point( 350, 140 ),
				new Point( 60, 300 ),
				new Point( 300, 250 ),
				new Point( 300, 330 )
			};
			AbstractInterpolation<Point, double> interpolation = new CardinalSplineInterpolation<Point, double>( keyPoints );

			// Create a set of interpolated points between the known points.
			var interpolatedPoints = new List<Point>();
			double curPercentage = 0;
			while ( curPercentage < 1 )
			{
				interpolatedPoints.Add( interpolation.Interpolate( curPercentage ) );
				curPercentage += 0.001;
			}

			// Create line segments.
			IEnumerable<LineSegment> lines = interpolatedPoints.Select( p => new LineSegment( p, true ) );

			// Create graph.
			var graphFigure = new PathFigure( keyPoints[ 0 ], lines, false );
			var graphGeometry = new PathGeometry( new List<PathFigure>
			{
				graphFigure
			} );
			var graph = new Path
			{
				Stroke = Brushes.Black,
				Data = graphGeometry
			};
			DrawCanvas.Children.Add( graph );

			// Create points.
			const double pointSize = 5;
			foreach ( var p in keyPoints )
			{
				var rect = new Rectangle
				{
					Width = pointSize,
					Height = pointSize,
					Fill = Brushes.Red
				};
				Canvas.SetLeft( rect, p.X - pointSize / 2 );
				Canvas.SetTop( rect, p.Y - pointSize / 2 );
				DrawCanvas.Children.Add( rect );
			}

			// Draw tangent at a desired location.
			const double tangentAtPercentage = 0.95;
			const double tangentLenght = 100;
			Point startTangent = interpolation.Interpolate( tangentAtPercentage );
			Point tangent = interpolation.TangentAt( tangentAtPercentage );
			var normalized = new Vector3D( tangent.X, tangent.Y, 0 );
			normalized.Normalize();
			tangent = new Point( normalized.X, normalized.Y );
			var tangentLine = new Line
			{
				X1 = startTangent.X,
				Y1 = startTangent.Y,
				X2 = startTangent.X + tangent.X * tangentLenght,
				Y2 = startTangent.Y + tangent.Y * tangentLenght,
				Stroke = Brushes.Blue
			};

			DrawCanvas.Children.Add( tangentLine );
		}
	}
}