   M S B A M L   `   `   `  ����I  EPRN212_Project, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullE'clr-namespace:PRN212_Project.ViewModelsPRN212_Project.ViewModels  5   	   3    -PRN212_Project.Views.TransactionHistoryWindow   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   6vm'clr-namespace:PRN212_Project.ViewModels     5   	     � Title$  Transaction History��5   	   $	��450��6%   $	��600��62    � WindowStartupLocation$ CenterScreen=�5	   	   .��5
      � $��10q�6    � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $��* �6   �� 5      $
��Auto �6   .+�5   
   �� $��0��6   $��24(�6?   $
��Bold"�6M   $��Center=�5      $��0,0,0,15q�61   ��5   !   �� *HistoryTitle+5   1   &   System.Windows.Controls.DataGrid  5   
   $��1��6      AutoGenerateColumns$ False��6E      
IsReadOnly$
 True��6a   $��0,0,0,10q�5      ��5       �� *Transactions+   Columns 5      0  *System.Windows.Controls.DataGridTextColumn  5      ,  &System.Windows.Controls.DataGridColumn   Header$
 Date��6%   1  +System.Windows.Controls.DataGridBoundColumn   Binding 63   �� *Date+ �� StringFormat$ d��  5      $
 Type��6%    63   �� *Type+  5      $ Category��6%    67   �� *Category.Name+  5      $ Amount��6%    65   �� *Amount+$ C��  5      $
 Note��6%    63   �� *Note+5      �� 5   
   $��2��6   $��Close��6   $	��100��6O   $��28��6[   $��Center=�5       $��0,10,0,0q�6.   ��5   .   �� *CloseCommand+5    .   5!      5"      