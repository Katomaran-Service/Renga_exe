   M S B A M L   `   `   `  ����B  >rengaas, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null!clr-namespace:rengaasrengaas  5   	       rengaas.startupscreen   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   'localclr-namespace:rengaas     5   	     � AllowsTransparency  .  � Title$ startupscreen��5	   	   $��669.376��6   $��429.727��60    � 
ResizeMode$ NoResize=�6@    � WindowStartupLocation$ CenterScreen=�6V    � WindowStyle$
 None=�6{    � Icon$ Rengas logo.png��6�   ��6�   Z�  � 	Resources 5
       	  close �� 
TargetType � 	Resources  Key	 �� TargetProperty
 �� 
TargetName8  2System.Windows.Media.Animation.EasingColorKeyFrame �� KeyTime �� Value �Name ��Name �� Triggers :� RoutedEvent :� 
SourceName ��Name  OnMouseEnter2  OnMouseLeave2��%U  &	        7  7  �� 5   
   " ��6(    5      ���� 5      OnMouseEnter2 .��5      �� $0	 *(Panel.Background).(SolidColorBrush.Color)�63   $

 grid��6z   .g�5        $ 0��6.   $	 Red��6:   5      5      �� 5      OnMouseLeave2 .��5      �� $0	 *(Panel.Background).(SolidColorBrush.Color)�63   $

 grid��6z   .g�5        $ 0��6.   $ Transparent��6:   5      5      5      .R�5      � $
 grid��6   
������ .+�5      �� $ image��6   $��close (1).png��6'   5       5      :� 5      $ UIElement.MouseEnter��6   $
 grid��6B   .[�5      �� $# OnMouseEnter2_BeginStoryboard��6&   8��  6M   5      :� 5       $ UIElement.MouseLeave��6   $
 grid��6B   .[�5!      �� $# OnMouseLeave2_BeginStoryboard��6&   8�� 6M   5"      5$      5'      .��5(      � -   5        $ 	startgrid��5(      $��0,0,10,0q�6    5)   
     FadeStoryboard W� From	 W� To g� AutoReverse W� EasingFunction.  (System.Windows.Media.Animation.CubicEase��%�   &	       �� 5*      .��5+      W� $ 2X�6"   $	 0.1X�6+   $	 Opacity�64   $
 	startgrid��5,   "    .  5.        5/      51      52      53       � RowDefinitions 54   
   �� 55      .+�57   
   �� -   5        $ image��57      $��0,0,0.426,-0.424q�6   $��First screen_exe.png��69    J� Effect 58      3  -System.Windows.Media.Effects.DropShadowEffect  59         	Direction$	 100Q�6#   5;      �� 5<   
   -   5         ��Name$ close��5<      $��Button��6    $
��Left=�61   $��31.262��6L   $��366.989,107.42,0,0q�6\   $	��Top=�6x   $��35.523��6�   #���   6�   6�   �� 5=   
   -   5         ��Name$ username_box��5=      $��109,288,131,346q�6(    �� TextWrapping$
 Wrap=�6A    �� TextAlignment$ Center=�6U   $��	User name��6l   
���   �
���   �$��15(�6�   $
��Bold"�6�   �� 5>   
   -   5         ��Name$ 	passlabel��5>      $��Password��6'    �� TextAlignment$ Center=�67   $��19��6N   $��109,0,131,300q�6Z   $��Bottom=�6q   
���   �$��15(�6�   $
��Bold"�6�   G� 5?   
   -   5          G�Name$  passbox��5?      $��36��6'   $��109,0,131,288q�63   $��Center=�6J   $��Bottom=�6n   
���   �
���   �6�   �� 5@   
   -   5        $ 	login_but��5@      $��LOG IN��6$   $��36��65   $��149,0,161,200q�6A   $��Bottom=�6X   ���	#FF4065DD���	#FF4065DD
�������6�   �� 5A   
   -   5        $ login_status��5A      $��30��6*   $
��Wrap=�66   $��Bottom=�6J   $��109,0,131,170q�6e   $ Center=�6|   �� 5B   
   -	   5        $ 	textBlock��5B      $ Center=�6'   $
��Left=�6>   $��34��6Y   $��
80,215,0,0q�6e   $
��Wrap=�6y   $��WELCOME TO DASHBOARD��6�   $	��Top=�6�   $	��262��6�   $��20(�6�   $
��Bold"�6�   
�������6�   �� 5C   
   -
   5        $ 
textBlock1��5C      $
��Left=�6(   $��955,237,-542,0q�6C   $
��Wrap=�6[   $��	TextBlock��6o   $	��Top=�6�   5D      5E      