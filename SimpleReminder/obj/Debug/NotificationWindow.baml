   M S B A M L   `   `   `  ����  SimpleReminder'    !SimpleReminder.NotificationWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	     � Title$  Напоминание��5   	    � SizeToContent$ WidthAndHeight=�5   	    � WindowStyle$
 None=�5   	    � 
ResizeMode$ NoResize=�5   	    � Topmost . 
������  � AllowsTransparency . .��5      ��  �� CornerRadius$ 10��6   
����������	#FFE0E0E0$��1q�5      $��10q�5       J� Effect 5   
   3  -System.Windows.Media.Effects.DropShadowEffect  5         
BlurRadius$ 15Q�6   	   ShadowDepth$	 3Q�6/   
   Color$
 	#40000000��6?   ._�5   
   �  � RowDefinitions 5      �� 5      $
��Auto �6    �� 5      $��* �6    �� 5      $
��Auto �6    .+�5      �� $��0��6   ���#009688$ 	10,10,0,0��5       �� Padding$ 15,10q�5       ._�5!      �� $��НАПОМИНАНИЕ��6   
�������$��16(�5#      $
��Bold"�5$      $��Center=�5%      5&      �� 5)      $��1��6   $��15q�6(   .��5*      �� .+�5+      �� -   5         ��Name$ CustomTitleTextBlock��5+       $
��Wrap=�5,      ���	#FF4285F4$��14(�5.      $
��Bold"�5/      $��0,0,0,5q�50      $y�	Collapsed=�51      �� 52      -   5        $ MessageTextBlock��52       $
��Wrap=�53      ���	#FF333333$��14(�55      56      57      �� 5:      $��2��6   $ 	0,0,10,10��5;      ���	#FFF5F5F5$ 10q�5=      ._�5>      �� -   5        $��ЗАКРЫТЬ��5>      $��Center=�5@      $	��120��5A      $��35��5B      ���#009688
�������$
��Bold"�5E      $��0q�5F      5G      5H      5I      5J      