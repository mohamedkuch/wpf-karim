   M S B A M L   `   `   `  ����Q  MExample_RealTime_Chart, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null?$clr-namespace:Example_RealTime_ChartExample_RealTime_Chart  5   	   U QLiveCharts.Wpf, Version=0.9.7.0, Culture=neutral, PublicKeyToken=0bc1f845d1ebb8dfG4clr-namespace:LiveCharts.Wpf;assembly=LiveCharts.WpfLiveCharts.Wpf 6<   '    !Example_RealTime_Chart.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   6local$clr-namespace:Example_RealTime_Chart     5   	   Dlvc4clr-namespace:LiveCharts.Wpf;assembly=LiveCharts.Wpf     6<     � Title$  
MainWindow��5   	   $	��450��6   $	��800��6)   .��5	      � .+�5
   
   �� .@�5      �� $
��Tab1��6   .��5      �  � ColumnDefinitions 5      �� 5      $��0.75* �6+   �� 5      $��0.25* �6+   .+�5      #  LiveCharts.Wpf.CartesianChart  $��0��6)   &   LiveCharts.Wpf.Charts.Base.Chart   AnimationsSpeed$ 0:0:0.5h�69      	Hoverable .     Zoom$ Xy=�6|   $��2��6�      DataTooltip 6e   Z�    Series 5        LiveCharts.Wpf.LineSeries     LineSmoothness$ 1Q�5   !     LiveCharts.Wpf.Series   StrokeThickness$ 6Q�5   !   	   Stroke	 �#F34336
   Fill

 ����    Values 5   -   �� *ChartValues+   PointGeometry 5   !   Z� 5   !      AxisX 5        LiveCharts.Wpf.Axis     LabelFormatter 6'   �� *DateTimeFormatter+   MaxValue 5      �� *	AxisMax+   MinValue 5       �� *	AxisMin+   Unit 5!      �� *
AxisUnit+   	Separator 5"   "     LiveCharts.Wpf.Separator  5#   &      Step 64   �� *
AxisStep+5%      5'      � 5(      $��1��6   .+�5)      �� .+�5*      �� $��Dateinummer:��6$   $
��Left=�6;   $��0q�6V   $	��Top=�6a   �� 5+      $��Center=�6X   $��23��6u   $��0q�6�    �� TextWrapping$
 Wrap=�6�   $	��Top=�6�   $	��120��6�   ��6&   ��  �� Path$ DataNumber_RT� �� Mode$ TwoWay=�6�   �� 5,      $��Anfang:��6$   $
��Left=�66   $��0q�6Q   $	��Top=�6\   �� 5-      $��Center=�6T   $��23��6q   $��0q�6}   $
 Wrap=�6�   $	��Top=�6�   $	��120��6�   ��6&   �� $ 	Anfang_RT�$ TwoWay=�6�   �� 5.      $��Ende:��6$   $
��Left=�64   $��0q�6O   $	��Top=�6Z   �� 5/      $��Center=�6R   $��23��6o   $��0q�6{   $
 Wrap=�6�   $	��Top=�6�   $	��120��6�   ��6&   �� $ Ende_RT�$ TwoWay=�6�   �� 50      $��Y-Axis:��6$   $
��Left=�66   $��0q�6Q   $	��Top=�6\   �� 51      $��Center=�6S   $��23��6p   $��0q�6|   $
 Wrap=�6�   $	��Top=�6�   $	��120��6�   ��6&   �� $ YAxis_RT�$ TwoWay=�6�   �� 53      -   5        $��0,20,0,0q�53   E   $��40��6W   .��6c   Generate realtime55      �� 56      -   5        $��0,20,0,0q�56   ?   $��40��6Q   .��6]   Inject/Stop Data58      5<      5?      5@      5A      �� 5B      $
��Tab2��6   .��5C      �  5E      �� 5F      $
��0.7* �6+   �� 5G      $
��0.3* �6+   .+�5J        $��0��6)      LegendLocation$ Right=�6^   $��10,10,17.2,48q�6u    J� RenderTransformOrigin$ 0.5,0.5&�6�   $ Xy=�6�    6:   �� *SeriesCollection+z�5K      V� 5L      .~�5M   "   �� �� 5N   "   �� 5O   "    �� Angle$ -0.113Q�62   T� 5P   "   5Q         AxisY 5T           Title$ Y1��6'    62   �� *
YFormatter+6Z    5W        $ X1��6'      Labels 62   �� *Labels+6N   5Y      � 5Z      $��1��6   .+�5[      �� .+�5\      �� $��Anfang:��6$   $
��Left=�66   $��0q�6Q   $	��Top=�6\   �� 5]      $��Center=�6U   $��23��6r   $��0q�6~   $
 Wrap=�6�   $	��Top=�6�   $	��120��6�   ��6&   �� $ 	Anfang_XY�$ TwoWay=�6�   �� 5^      $��Ende:��6$   $
��Left=�64   $��0q�6O   $	��Top=�6Z   �� 5_      $��Center=�6S   $��23��6p   $��0q�6|   $
 Wrap=�6�   $	��Top=�6�   $	��120��6�   ��6&   �� $ Ende_XY�$ TwoWay=�6�   �� 5`      $��Dateinummer:��6$   $
��Left=�6;   $��0q�6V   $	��Top=�6a   �� 5a      $��Center=�6X   $��23��6u   $��0q�6�   $
 Wrap=�6�   $	��Top=�6�   $	��120��6�   ��6&   �� $ DataNumber_XY�$ TwoWay=�6�   �� 5b      $��X-Axis:��6$   $
��Left=�66   $��0q�6Q   $	��Top=�6\   �� 5c      $��Center=�6R   $��23��6o   $��0q�6{   $
 Wrap=�6�   $	��Top=�6�   $	��120��6�   ��6&   �� $ XAxis_XY�$ TwoWay=�6�   �� 5d      $��Y-Axis:��6$   $
��Left=�66   $��0q�6Q   $	��Top=�6\   �� 5e      $��Center=�6R   $��23��6o   $��0q�6{   $
 Wrap=�6�   $	��Top=�6�   $	��120��6�   ��6&   �� $ YAxis_XY�$ TwoWay=�6�   �� 5f      -   5        $��Generate XY��5f   %   $��Center=�6U   $��0,30,0,0q�6r   $	��Top=�6�   $	��203��6�   $��53��6�   �� 5g      -   5        $��0,5,0,0q�5g   :   $	��203��6K   $��Center=�6W   $	��Top=�6t   $��53��6�   $
��Plot��6�   5i      5l      5m      5n      5o      5p      5q      