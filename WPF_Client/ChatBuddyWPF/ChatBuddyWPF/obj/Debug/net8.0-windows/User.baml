   M S B A M L   `   `   `  ����G  CChatBuddyWPF, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null+clr-namespace:ChatBuddyWPFChatBuddyWPF  5          ChatBuddyWPF.User   5      W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5      R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      ,localclr-namespace:ChatBuddyWPF     5        N� Title$
  User��5	      .��5      � .+�5   
   �� $	��600��6   $	��400��5      
������� �� CornerRadius$ 6��5       J� Effect 5      3  -System.Windows.Media.Effects.DropShadowEffect  5         
BlurRadius$ 15Q�6#      	Direction$	 -90Q�5   #      RenderingBias$ Quality=�5   #      ShadowDepth$ 2Q�5   #      Color$
 Gray��5   #   ._�5      �  � RowDefinitions 5      �� 5      $
��Auto �6$   �� 5      $��* �6$   .+�5      � 	 � ColumnDefinitions	 5       �� 5!      $
��Auto �6+   �� 5"      $��* �6+   .+�5&      �� -   5        $��Create Chat��5&      $
��Left=�5'      $��Center=�5(      $��10,0,0,0q�5)      $��2��5*   <   $��33��6P   $	��123��6\      CustomButtonStyle#��[  6h   �� 5+      $��Chats��6    $��25(�5,       $��Center=�5-       $	��Top=�5.       $
��Bold"�5/       
 �� Padding$
 10q�50       $��1��51       52      �� 53      $
��Auto=�6   $��Disabled=�6B   $��1��6k   .��54      �� -   5         ��Name$ ButtonContainer��54   !    �� Orientation$ Vertical=�6:   55      56      57      58      59      