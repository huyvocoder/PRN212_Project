   M S B A M L   `   `   `  ����I  EPRN212_Project, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null;"clr-namespace:PRN212_Project.ViewsPRN212_Project.Views  5   	   E'clr-namespace:PRN212_Project.ViewModelsPRN212_Project.ViewModels  5   	   )    #PRN212_Project.Views.OverviewWindow   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   4local"clr-namespace:PRN212_Project.Views     5   	   6vm'clr-namespace:PRN212_Project.ViewModels     5   	     � Title$  Financial Overview��5	   	   $	��600��6$   $	��800��61    � 	Resources 5
       �� 
TargetType   CategoryBoxStyle �� Property �� Value �� Triggers �� Binding �� Value��%�  &	        �� 5   
   " ��6   .�5      ��  � ��
 �����6+   �� 5       � ��
 �����6,   �� 5       � ��$ 1q�60   �� 5       �@��CornerRadius$ 5��6-   �� 5       �@��Padding$ 10q�6(   �� 5       � ��$ 5q�6'   �� 5       � ��$	 150��6&   �� 5       � ��$	 120��6'    5      �� 5       6   �� *BackgroundColor+ LightYellow6B   .a�5      ��  � ��
 �����63   �� 5       � ��
 � ���64   5      �� 5       6   �� *BackgroundColor+ 
LightCoral6B   .a�5      ��  � ��
 �����63   �� 5       � ��
 �  ��64   5      5      5       � DataContext 5       1   +PRN212_Project.ViewModels.OverviewViewModel  5!   
   .��5#      � $��10q�6   	 � RowDefinitions	 5$   
   �� 5%      $
��Auto �6   �� 5&      $
��Auto �6   �� 5'      $��* �6   �� 5(      $
��Auto �6   .+�5,   
   � $��0��6   $��0,0,0,20q�6   
 � ColumnDefinitions
 5-      �� 5.      $��* �6#   �� 5/      $
��Auto �6#   �� 50      $
��Auto �6#   �� 51      $
��Auto �6#   .+�54      ��  �� Orientation$ 
Horizontal=�6   $��0��62   $��Center=�6B   .+�55      �� $��32(�6V   $
��Bold"�6d   $��Center=�6v   ��6   �� *	Balance+ �� StringFormat$ Total Balance: {0}��6v   �� 56      $��+��6   $��30��6J   $��30��6U   $��20(�6a   $
��Bold"�6o   $��10,0,5,0q�6�   $��Center=�6�   ��6%   �� *AddIncomeCommand+6�   �� 57      $	��⏳��6   $��30��6R   $��30��6]   $��14(�6i   $��5,0,0,0q�6w   $��Center=�6�   ��6%   �� *ShowIncomeHistoryCommand+6�   58      �� 5:      $��1��6   $��Change Password��6%   $	��120��6i   $��30��6u   $	��5,0q�6�   ��6?   �� *ChangePasswordCommand+6�   �� 5;      $��2��6   $��Logout��6%   $��80��6X   $��30��6c   $	��5,0q�6o   ��66   �� *LogoutCommand+6o   5<      � 5?   
   $��1��6   $��0,0,0,10q�6   
 5@      �� 5A      $��* �6#   �� 5B      $
��Auto �6#   .+�5D      �� $��0��6   $��
Categories��6$   $
��Bold"�69   $��18(�6K   $��Center=�6Y   �� 5E      $��1��6   $��Add Category��6%   $	��100��6c   $��28��6o   ��6<   �� *AddCategoryCommand+6o   5F      �� 5I   
   $��2��6   ��6$   �� *
Categories+��5J      �� 5K      .R�5L      �  � Orientation$ 
Horizontal=�6    5M      ��5O      �� 5P      .R�5Q      �� #��[  6   ._�5R      �� .+�5S      �� $
��Bold"�6>   $��16(�6P   $��Center=�6^   $��0,0,0,5q�6{   ��6(   �� *Name+6{   �� 5T      $��Center=�6_   ��6(   �� *CurrentSpent+$ 
Spent: {0}��6_   �� 5U      $��Center=�6_   $��0,0,0,10q�6|   ��6(   �� *MonthlyLimit+$ 
Limit: {0}��6|   �� 5V      $ 
Horizontal=�6)   $��Center=�6B   .+�5W   "   �� $��+��6)   $��30��6�   $��25��6�   $��14(�6�   $��2q�6�   ��65   �� *DataContext.AddExpenseCommand+ �� RelativeSource ��  �� AncestorType ItemsControl��6�   �� *Id+6�   �� 5X   "   $	��⏳��6)   $��30��6�   $��25��6�   $��14(�6�   $��2q�6�   ��65   �� *(&DataContext.ShowCategoryHistoryCommand+ ��  ItemsControl��6�   �� *Id+6�   5Y      5Z      5[      5\      5^      �� 5a   
   $��3��6   $��Show Income History��6   $	��150��6i   $��30��6u   $��0,20,0,0q�6�   $��Center=�6�   ��6<   �� *ShowIncomeHistoryCommand+6�   5b      5c      