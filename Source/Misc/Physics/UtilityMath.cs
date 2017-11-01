using UnityEngine;
using System;
using System.Collections.Generic;

namespace PeenToys
{
	//##########################
	// Class Declaration
	//##########################
	/// <summary>Utility for various math functions</summary>
	public static class UtilityMath
	{
		//=======================
		// Vector3
		//=======================
		/// <summary>Extension method that rounds <see cref="UnityEngine.Vector3"/> components</summary>
		/// <param name="tVector">Vector to round</param>
		/// <returns>A rounded vector</returns>
		public static Vector3 Round( this Vector3 tVector )
		{
			return new Vector3( Mathf.Round( tVector.x ), Mathf.Round( tVector.y ), Mathf.Round( tVector.z ) );
		}
		
		//=======================
		// Vector2
		//=======================
		/// <summary>Extension method that finds the closest point between a single <see cref="UnityEngine.Vector2"/> and an array of points</summary>
		/// <param name="tVector">Vector to test</param>
		/// <param name="tPoints">Array of points to test against</param>
		/// <returns>Closest vector point</returns>
		public static Vector2 ClosestPoint( this Vector2 tPoint, Vector2[] tPoints )
		{
			if ( tPoints != null )
			{
				int tempShortest = tPoints.Length - 1;
				float tempShortestDistance = ( tPoint - tPoints[ tempShortest ] ).magnitude;
				float tempMagnitude;
				for ( int i = ( tempShortest - 1 ); i >= 0; --i )
				{
					tempMagnitude = ( tPoint - tPoints[i] ).magnitude;
					if ( tempMagnitude < tempShortestDistance )
					{
						tempShortest = i;
						tempShortestDistance = tempMagnitude;
					}
				}
				
				return tPoints[ tempShortest ];
			}
			
			return tPoint;
		}
		
		//=======================
		// Intersections
		//=======================
		/// <summary>Finds the intersections between a circle and circle</summary>
		/// <param name="tCenterA">Center of circle A</param>
		/// <param name="tRadiusA">Radius of circle A</param>
		/// <param name="tCenterB">Center of circle B</param>
		/// <param name="tRadiusB">Radius of circle B</param>
		/// <param name="tIntersections">Output of intersecting points</param>
		/// <returns>True if collided</returns>
		public static bool CircleVsCircle( Vector2 tCenterA, float tRadiusA, Vector2 tCenterB, float tRadiusB, out Vector2[] tIntersections )
		{
			float tempDistance = ( tCenterA - tCenterB ).magnitude;
			if ( tempDistance > 0 && tempDistance <= ( tRadiusA + tRadiusB ) && tempDistance >= Mathf.Abs( tRadiusA - tRadiusB ) )
			{
				// Find triangle
				float tempAdjacent = ( tRadiusA * tRadiusA - tRadiusB * tRadiusB + tempDistance * tempDistance ) / ( 2 * tempDistance );
				float tempHyp = Mathf.Sqrt( ( tRadiusA * tRadiusA ) - ( tempAdjacent * tempAdjacent ) );
				float tempMiddleX = tCenterA.x + tempAdjacent * ( tCenterB.x - tCenterA.x ) / tempDistance;
				float tempMiddleY = tCenterA.y + tempAdjacent * ( tCenterB.y - tCenterA.y ) / tempDistance;

				// Fill intersection points
				Vector2 tempIntersection0 = new Vector2( ( tempMiddleX + tempHyp * ( tCenterB.y - tCenterA.y ) / tempDistance ), ( tempMiddleY - tempHyp * ( tCenterB.x - tCenterA.x ) / tempDistance ) );
				if ( tempDistance == ( tRadiusA + tRadiusB ) )
				{
					tIntersections = new Vector2[1];
					tIntersections[0] = tempIntersection0;
				}
				else
				{
					tIntersections = new Vector2[2];
					tIntersections[0] = tempIntersection0;
					tIntersections[1] = new Vector2( ( tempMiddleX - tempHyp * ( tCenterB.y - tCenterA.y ) / tempDistance ), ( tempMiddleY + tempHyp * ( tCenterB.x - tCenterA.x ) / tempDistance ) );
				}
				
				return true;
			}
			
			tIntersections = null;
			return false;
		}
		
		/// <summary>Finds the intersecton between a segment and segment</summary>
		/// <param name="tLineAStart">Line A start</param>
		/// <param name="tLineAEnd">Line A end</param>
		/// <param name="tLineBStart">Line B start</param>
		/// <param name="tLineBEnd">Line B end</param>
		/// <param name="tIntersection">Output of intersection point</param>
		/// <returns>True if intersection occurs</returns>
		public static bool SegmentVsSegment( Vector2 tLineAStart, Vector2 tLineAEnd, Vector2 tLineBStart, Vector2 tLineBEnd, out Vector2 tIntersection )
		{
			if ( SegmentVsLine( tLineAStart, tLineAEnd, tLineBStart, tLineBEnd, out tIntersection )
				&& Mathf.Min( tLineBStart.x, tLineBEnd.x ) <= tIntersection.x && tIntersection.x <= Mathf.Max( tLineBStart.x, tLineBEnd.x )
				&& Mathf.Min( tLineBStart.y, tLineBEnd.y ) <= tIntersection.y && tIntersection.y <= Mathf.Max( tLineBStart.y, tLineBEnd.y ) )
			{
				return true;
			}
			
			return false;
		}
		
		/// <summary>Finds the intersecton between a segment and line</summary>
		/// <param name="tLineAStart">Line A start</param>
		/// <param name="tLineAEnd">Line A end</param>
		/// <param name="tLineBStart">Line B start</param>
		/// <param name="tLineBEnd">Line B end</param>
		/// <param name="tIntersection">Output of intersection point</param>
		/// <returns>True if intersection occurs</returns>
		public static bool SegmentVsLine( Vector2 tLineAStart, Vector2 tLineAEnd, Vector2 tLineBStart, Vector2 tLineBEnd, out Vector2 tIntersection )
		{
			if ( LineVsLine( tLineAStart, tLineAEnd, tLineBStart, tLineBEnd, out tIntersection )
				&& Mathf.Min( tLineAStart.x, tLineAEnd.x ) <= tIntersection.x && tIntersection.x <= Mathf.Max( tLineAStart.x, tLineAEnd.x )
				&& Mathf.Min( tLineAStart.y, tLineAEnd.y ) <= tIntersection.y && tIntersection.y <= Mathf.Max( tLineAStart.y, tLineAEnd.y ) )
			{
				return true;
			}
			
			return false;
		}
		
		/// <summary>Finds the intersecton between a line and line</summary>
		/// <param name="tLineAStart">Line A start</param>
		/// <param name="tLineAEnd">Line A end</param>
		/// <param name="tLineBStart">Line B start</param>
		/// <param name="tLineBEnd">Line B end</param>
		/// <param name="tIntersection">Output of intersection point</param>
		/// <returns>True if intersection occurs</returns>
		public static bool LineVsLine( Vector2 tLineAStart, Vector2 tLineAEnd, Vector2 tLineBStart, Vector2 tLineBEnd, out Vector2 tIntersection )
		{
			float tempDeterminant = ( ( tLineAStart.x - tLineAEnd.x ) * ( tLineBStart.y - tLineBEnd.y ) ) - ( ( tLineAStart.y - tLineAEnd.y ) * ( tLineBStart.x - tLineBEnd.x ) );
			if ( tempDeterminant != 0 )
			{
				tempDeterminant = 1 / tempDeterminant;
				tIntersection = new Vector2( 
					( ( ( ( tLineAStart.x * tLineAEnd.y ) - ( tLineAStart.y * tLineAEnd.x ) ) * ( tLineBStart.x - tLineBEnd.x ) ) - ( ( ( tLineBStart.x * tLineBEnd.y ) - ( tLineBStart.y * tLineBEnd.x ) ) * ( tLineAStart.x - tLineAEnd.x ) ) ) * tempDeterminant,
					( ( ( ( tLineAStart.x * tLineAEnd.y ) - ( tLineAStart.y * tLineAEnd.x ) ) * ( tLineBStart.y - tLineBEnd.y ) ) - ( ( ( tLineBStart.x * tLineBEnd.y ) - ( tLineBStart.y * tLineBEnd.x ) ) * ( tLineAStart.y - tLineAEnd.y ) ) ) * tempDeterminant );
				
				return true;
			}
			
			tIntersection = Vector2.zero;
			return false;
		}
		
		/// <summary>Finds the intersecton between a segment and rectangle via Liang-Barsky</summary>
		/// <param name="tLineStart">Line start</param>
		/// <param name="tLineEnd">Line end</param>
		/// <param name="tRectBottomLeft">Bottom-left corner of a rectangle</param>
		/// <param name="tRectTopRight">Top-right corner of a rectangle</param>
		/// <param name="tIntersections">Output of intersection point</param>
		/// <returns>True if intersection occurs</returns>
		public static bool SegmentVsRectangle( Vector2 tLineStart, Vector2 tLineEnd, Vector2 tRectBottomLeft, Vector2 tRectTopRight, out Vector2[] tIntersections )
		{
			if ( tRectBottomLeft != tRectTopRight )
			{
				float tempDeltaX = tLineEnd.x - tLineStart.x;
				float tempU1 = 0;
				float tempU2 = 1;
				if ( SegmentClipTest( -tempDeltaX, ( tLineStart.x - tRectBottomLeft.x ), ref tempU1, ref tempU2 ) && SegmentClipTest( tempDeltaX, ( tRectTopRight.x - tLineStart.x ), ref tempU1, ref tempU2 ) )
				{
					float tempDeltaY = tLineEnd.y - tLineStart.y;
					if ( SegmentClipTest( -tempDeltaY, ( tLineStart.y - tRectBottomLeft.y ), ref tempU1, ref tempU2 ) && SegmentClipTest( tempDeltaY, ( tRectTopRight.y - tLineStart.y ), ref tempU1, ref tempU2 ) )
					{
						// Same intersection
						if ( tempU1 == tempU2 )
						{
							tIntersections = new Vector2[1];
							tIntersections[0] = new Vector2( ( tLineStart.x + ( tempU1 * tempDeltaX ) ), ( tLineStart.y + ( tempU1 * tempDeltaY ) ) );
							
							return true;
						}
						// Two intersections
						else if ( tempU2 < 1 && tempU1 > 0 )
						{
							tIntersections = new Vector2[2];
							tIntersections[0] = new Vector2( ( tLineStart.x + ( tempU1 * tempDeltaX ) ), ( tLineStart.y + ( tempU1 * tempDeltaY ) ) );
							tIntersections[1] = new Vector2( ( tLineStart.x + ( tempU2 * tempDeltaX ) ), ( tLineStart.y + ( tempU2 * tempDeltaY ) ) );
							
							return true;
						}
						// One intersection
						else if ( tempU1 > 0 )
						{
							tIntersections = new Vector2[1];
							tIntersections[0] = new Vector2( ( tLineStart.x + ( tempU1 * tempDeltaX ) ), ( tLineStart.y + ( tempU1 * tempDeltaY ) ) );
							
							return true;
						}
						// One intersection
						else if ( tempU2 < 1 )
						{
							tIntersections = new Vector2[1];
							tIntersections[0] = new Vector2( ( tLineStart.x + ( tempU2 * tempDeltaX ) ), ( tLineStart.y + ( tempU2 * tempDeltaY ) ) );
							
							return true;
						}
					}
				}
			}

			tIntersections = null;
			return false;
		}
		
		/// <summary>Helper function that determines if a segment passes the Liang-Barsky clipping test</summary>
		/// <param name="tDelta">Segment dimension delta</param>
		/// <param name="tSegmentToRect">Dimension difference between segment and rect coordinates</param>
		/// <param name="tU1">Beginning of the segment</param>
		/// <param name="tU2">End of the segment</param>
		/// <returns>True if the segment successfully passed</returns>
		public static bool SegmentClipTest( float tDelta, float tSegmentToRect, ref float tU1, ref float tU2 )
		{
			// Outside to inside
			if ( tDelta < 0 )
			{
				float tempR = tSegmentToRect / tDelta;
				if ( tempR > tU2 )
				{
					return false;
				}
				else if ( tempR > tU1 )
				{
					tU1 = tempR;
				}
			}
			// Inside to outside
			else if ( tDelta > 0 )
			{
				float tempR = tSegmentToRect / tDelta;
				if ( tempR < tU1 )
				{
					return false;
				}
				else if ( tempR < tU2 )
				{
					tU2 = tempR;
				}
			}
			// Outside
			else if ( tSegmentToRect < 0 )
			{
				return false;
			}
			
			return true;
		}
		
		/// <summary>Finds the intersectons between a segment and ellipse (assumed axis-aligned)</summary>
		/// <param name="tLineStart">Line start</param>
		/// <param name="tLineEnd">Line end</param>
		/// <param name="tEllipseCenter">Center of the ellipse</param>
		/// <param name="tEllipseWidth">Width of the ellipse</param>
		/// <param name="tEllipseHeight">Height of the ellipse</param>
		/// <param name="tIntersections">Output of intersecting points</param>
		/// <returns>True if intersection occurs</returns>
		public static bool SegmentVsEllipse( Vector2 tLineStart, Vector2 tLineEnd, Vector2 tEllipseCenter, float tEllipseWidth, float tEllipseHeight, out Vector2[] tIntersections )
		{
			if ( tEllipseWidth != 0 && tEllipseHeight != 0 && tLineStart != tLineEnd )
			{
				tLineStart -= tEllipseCenter;
				tLineEnd -= tEllipseCenter;
				tEllipseWidth *= 0.5f;
				tEllipseWidth *= tEllipseWidth;
				tEllipseHeight *= 0.5f;
				tEllipseHeight *= tEllipseHeight;
				
				// Calculate quadratic components
				float tempA = ( ( ( tLineEnd.x * tLineEnd.x ) + ( tLineStart.x * tLineStart.x ) - ( 2 * tLineStart.x * tLineEnd.x ) ) / tEllipseWidth ) + ( ( ( tLineEnd.y * tLineEnd.y ) + ( tLineStart.y * tLineStart.y ) - ( 2 * tLineStart.y * tLineEnd.y ) ) / tEllipseHeight );
				float tempB = ( ( ( 2 * tLineStart.x * tLineEnd.x ) - ( 2 * tLineStart.x * tLineStart.x ) ) / tEllipseWidth ) + ( ( ( 2 * tLineStart.y * tLineEnd.y ) - ( 2 * tLineStart.y * tLineStart.y ) ) / tEllipseHeight );
				float tempC = ( ( tLineStart.x * tLineStart.x ) / tEllipseWidth ) + ( ( tLineStart.y * tLineStart.y ) / tEllipseHeight ) - 1;
				float tempDeterminant = ( tempB * tempB ) - ( 4 * tempA * tempC );
				
				if ( tempDeterminant >= 0 )
				{
					// Find discriminants
					List<float> tempValues = new List<float>();
					List<Vector2> tempPoints = null;
					if ( tempDeterminant == 0 )
					{
						tempValues.Add( -tempB / ( 2 * tempA ) );
					}
					else
					{
						tempValues.Add( ( -tempB + Mathf.Sqrt( tempDeterminant ) ) / ( 2 * tempA ) );
						tempValues.Add( ( -tempB - Mathf.Sqrt( tempDeterminant ) ) / ( 2 * tempA ) );
					}

					// Fill intersection points
					for ( int i = ( tempValues.Count - 1 ); i >= 0; --i )
					{
						if ( tempValues[i] >= 0 && tempValues[i] <= 1 )
						{
							if ( tempPoints == null )
							{
								tempPoints = new List<Vector2>();
							}
							tempPoints.Add( new Vector2( ( tEllipseCenter.x + tLineStart.x + ( ( tLineEnd.x - tLineStart.x ) * tempValues[i] ) ), ( tEllipseCenter.y + tLineStart.y + ( ( tLineEnd.y - tLineStart.y ) * tempValues[i] ) ) ) );
						}
					}
					
					// Converts to static array
					if ( tempPoints != null )
					{
						tIntersections = new Vector2[ tempPoints.Count ];
						for ( int i = ( tempPoints.Count - 1 ); i >= 0; --i )
						{
							tIntersections[i] = tempPoints[i];
						}

						return true;
					}
				}
			}
			
			tIntersections = null;
			return false;
		}
		
		/// <summary>Finds the intersectons between a line and ellipse (assumed axis-aligned)</summary>
		/// <param name="tLineStart">Line start</param>
		/// <param name="tLineEnd">Line end</param>
		/// <param name="tEllipseCenter">Center of the ellipse</param>
		/// <param name="tEllipseWidth">Width of the ellipse</param>
		/// <param name="tEllipseHeight">Height of the ellipse</param>
		/// <param name="tIntersections">Output of intersecting points</param>
		/// <returns>True if intersection occurs</returns>
		public static bool LineVsEllipse( Vector2 tLineStart, Vector2 tLineEnd, Vector2 tEllipseCenter, float tEllipseWidth, float tEllipseHeight, out Vector2[] tIntersections )
		{
			if ( tEllipseWidth != 0 && tEllipseHeight != 0 && tLineStart != tLineEnd )
			{
				tLineStart -= tEllipseCenter;
				tLineEnd -= tEllipseCenter;
				tEllipseWidth *= 0.5f;
				tEllipseWidth *= tEllipseWidth;
				tEllipseHeight *= 0.5f;
				tEllipseHeight *= tEllipseHeight;
				
				// Calculate quadratic components
				float tempA = ( ( ( tLineEnd.x * tLineEnd.x ) + ( tLineStart.x * tLineStart.x ) - ( 2 * tLineStart.x * tLineEnd.x ) ) / tEllipseWidth ) + ( ( ( tLineEnd.y * tLineEnd.y ) + ( tLineStart.y * tLineStart.y ) - ( 2 * tLineStart.y * tLineEnd.y ) ) / tEllipseHeight );
				float tempB = ( ( ( 2 * tLineStart.x * tLineEnd.x ) - ( 2 * tLineStart.x * tLineStart.x ) ) / tEllipseWidth ) + ( ( ( 2 * tLineStart.y * tLineEnd.y ) - ( 2 * tLineStart.y * tLineStart.y ) ) / tEllipseHeight );
				float tempC = ( ( tLineStart.x * tLineStart.x ) / tEllipseWidth ) + ( ( tLineStart.y * tLineStart.y ) / tEllipseHeight ) - 1;
				float tempDeterminant = ( tempB * tempB ) - ( 4 * tempA * tempC );
				
				if ( tempDeterminant >= 0 )
				{
					// Find discriminants
					List<float> tempValues = new List<float>();
					List<Vector2> tempPoints = null;
					if ( tempDeterminant == 0 )
					{
						tempValues.Add( -tempB / ( 2 * tempA ) );
					}
					else
					{
						tempValues.Add( ( -tempB + Mathf.Sqrt( tempDeterminant ) ) / ( 2 * tempA ) );
						tempValues.Add( ( -tempB - Mathf.Sqrt( tempDeterminant ) ) / ( 2 * tempA ) );
					}

					// Fill intersection points
					for ( int i = ( tempValues.Count - 1 ); i >= 0; --i )
					{
						if ( tempPoints == null )
						{
							tempPoints = new List<Vector2>();
						}
						tempPoints.Add( new Vector2( ( tEllipseCenter.x + tLineStart.x + ( ( tLineEnd.x - tLineStart.x ) * tempValues[i] ) ), ( tEllipseCenter.y + tLineStart.y + ( ( tLineEnd.y - tLineStart.y ) * tempValues[i] ) ) ) );
					}
					
					// Converts to static array
					if ( tempPoints != null )
					{
						tIntersections = new Vector2[ tempPoints.Count ];
						for ( int i = ( tempPoints.Count - 1 ); i >= 0; --i )
						{
							tIntersections[i] = tempPoints[i];
						}

						return true;
					}
				}
			}
			
			tIntersections = null;
			return false;
		}
		
		//=======================
		// Distance
		//=======================
		/// <summary>Finds closest distance between a point and a polygon</summary>
		/// <param name="tPoint">Point to test</param>
		/// <param name="tPolyPoints">Polygon vertices</param>
		/// <param name="tIntersection">Closest point along the polygon edge to the <paramref name="tPoint"/></param>
		/// <returns>Distance between <paramref name="tPoint"/> and the closest point on the polygon edge</returns>
		public static float DistanceFromPoly( Vector2 tPoint, Vector2[] tPolyPoints, out Vector2 tIntersection )
		{
			if ( tPolyPoints != null )
			{
				int tempListLength = tPolyPoints.Length - 1;
				if ( tempListLength > 0 )
				{
					Vector2 tempOut;
					float tempDistance = DistanceFromSegment( tPoint, tPolyPoints[0], tPolyPoints[ tempListLength ], out tIntersection );
					if ( tempListLength > 1 )
					{
						float tempShortest = tempDistance;
						for ( int i = tempListLength; i >= 1; --i )
						{
							tempDistance = DistanceFromSegment( tPoint, tPolyPoints[i], tPolyPoints[ i - 1 ], out tempOut );
							if ( tempDistance < tempShortest )
							{
								tempShortest = tempDistance;
								tIntersection = tempOut;
							}
						}
						
						return tempShortest;
					}
					
					return tempDistance;
				}
			}
			
			tIntersection = tPoint;
			return 0;
		}
		
		/// <summary>Finds closest distance between a point and a line segment</summary>
		/// <param name="tPoint">Point to test</param>
		/// <param name="tLineStart">Line start</param>
		/// <param name="tLineEnd">Line end</param>
		/// <param name="tIntersection">Closest point along the line to <paramref name="tPoint"/></param>
		/// <returns>Distance between <paramref name="tPoint"/> and the closest point along the segment</returns>
		public static float DistanceFromSegment( Vector2 tPoint, Vector2 tLineStart, Vector2 tLineEnd, out Vector2 tIntersection )
		{
			if ( tLineStart == tLineEnd ) 
			{
				tIntersection = tLineStart;
			}
			else
			{
				Vector2 tempAB = tLineEnd - tLineStart;
				float tempDistanceNormalized = ( ( ( tPoint.x - tLineStart.x ) * tempAB.x ) + ( ( tPoint.y - tLineStart.y ) * tempAB.y ) ) / ( ( tempAB.x * tempAB.x ) + ( tempAB.y * tempAB.y ) );
				if ( tempDistanceNormalized > 0 && tempDistanceNormalized < 1 )
				{
					tIntersection = new Vector2( ( tLineStart.x + ( tempAB.x * tempDistanceNormalized ) ), ( tLineStart.y + ( tempAB.y * tempDistanceNormalized ) ) );
				}
				else if ( tempDistanceNormalized <= 0 )
				{
					tIntersection = tLineStart;
				}
				else
				{
					tIntersection = tLineEnd;
				}
			}
			
			return ( tPoint - tIntersection ).magnitude;
		}
		
		/// <summary>Finds closest distance between a point and a line</summary>
		/// <param name="tPoint">Point to test</param>
		/// <param name="tLineStart">Line start</param>
		/// <param name="tLineEnd">Line end</param>
		/// <param name="tIntersection">Closest point along the line to <paramref name="tPoint"/></param>
		/// <returns>Distance between <paramref name="tPoint"/> and the closest point along the line</returns>
		public static float DistanceFromLine( Vector2 tPoint, Vector2 tLineStart, Vector2 tLineEnd, out Vector2 tIntersection )
		{
			if ( tLineStart == tLineEnd ) 
			{
				tIntersection = tLineStart;
			}
			else
			{
				tLineEnd -= tLineStart;
				float tempDistanceNormalized = ( ( ( tPoint.x - tLineStart.x ) * tLineEnd.x ) + ( ( tPoint.y - tLineStart.y ) * tLineEnd.y ) ) / ( ( tLineEnd.x * tLineEnd.x ) + ( tLineEnd.y * tLineEnd.y ) );
				tIntersection = new Vector2( ( tLineStart.x + ( tLineEnd.x * tempDistanceNormalized ) ), ( tLineStart.y + ( tLineEnd.y * tempDistanceNormalized ) ) );
			}
			
			return ( tPoint - tIntersection ).magnitude;
		}
	}
}