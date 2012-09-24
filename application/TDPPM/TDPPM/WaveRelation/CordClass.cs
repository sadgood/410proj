using System;
using System.Drawing;
using System.Collections;

namespace WaveRelationControl
{
	/// <summary>
	/// CordClass 的摘要说明。
	/// </summary>
	public class CordClass
	{
		private Point _StartDrawPoint, _EndDrawPoint;
		private PointF _RealStartPoint, _RealEndPoint;


		private float XStep;
		private float YStep;


		public CordClass(Point StartDrawPoint, Point EndDrawPoint,PointF RealStartPoint,PointF RealEndPoint)
		{
			//
			// TODO: 在此处添加构造函数逻辑
			//
			_StartDrawPoint = StartDrawPoint;
			_EndDrawPoint = EndDrawPoint;

			_RealStartPoint =  RealStartPoint;
			_RealEndPoint  = RealEndPoint;

			// XStep = (RealEndPoint.X - RealStartPoint.X)/(EndDrawPoint.X - StartDrawPoint.X);
			YStep = (RealEndPoint.Y - RealStartPoint.Y)/(EndDrawPoint.Y - StartDrawPoint.Y);
            XStep = (RealEndPoint.X - RealStartPoint.X) / (EndDrawPoint.X - StartDrawPoint.X);
		}

		public Point ConvertRealToDraw(PointF RealPoint)
		{
            int X = _StartDrawPoint.X + (int)((RealPoint.X - _RealStartPoint.X) / XStep);
			int Y = _StartDrawPoint.Y +  (int)((RealPoint.Y - _RealStartPoint.Y)/YStep);

			return new Point(X,Y);
		}

		// 将实际值转换为绘图点
		public Point[] ConvertRealPointsToDrawPoints(PointF[] ControlPoints)
		{
			ArrayList AL = new ArrayList();

			SolidBrush DrawBrush = new SolidBrush(Color.Blue);

			if(ControlPoints!=null)
			{
                for (int i = 0; i < ControlPoints.Length; i++)
                {
                    Point DPoint = ConvertRealToDraw(ControlPoints[i]);
                    AL.Add(DPoint);
                }
			}


			Point[] ReturnPoints = new Point[AL.Count];

			for (int i = 0; i < AL.Count; i++)
			{
				ReturnPoints[i] = (Point)AL[i];
			}

			return ReturnPoints;
		}


		public PointF ConvertDrawToReal(Point DrawPoint)
		{
			float X = (DrawPoint.X - _StartDrawPoint.X) * XStep + _RealStartPoint.X;
			float Y = (DrawPoint.Y - _StartDrawPoint.Y) * YStep + _RealStartPoint.Y;

			return new PointF(X, Y);

		}

		public float GetYDis()
		{
			return Math.Abs(_RealEndPoint.Y - _RealStartPoint.Y);
		}



	}
}
