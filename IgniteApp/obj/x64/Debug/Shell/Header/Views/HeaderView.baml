   M S B A M L   `   `   `  ����D  @IgniteApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null9!clr-namespace:IgniteApp.BehaviorsIgniteApp.Behaviors  5      K*clr-namespace:IgniteApp.Shell.Header.ViewsIgniteApp.Shell.Header.Views  5      -    'IgniteApp.Shell.Header.Views.HeaderView   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      0be!clr-namespace:IgniteApp.Behaviors     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      _ [Microsoft.Xaml.Behaviors, Version=1.1.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a3i+http://schemas.microsoft.com/xaml/behaviors  5      6local*clr-namespace:IgniteApp.Shell.Header.Views  5      Dmc;http://schemas.openxmlformats.org/markup-compatibility/2006  5	      .��5      � -   5          �Name$  
GridButton��5      ���	#FF0078D7*  $Microsoft.Xaml.Behaviors.Interaction   	Behaviors 5   
   &    IgniteApp.Behaviors.DragBehavior  5      .+�5   
   �� $��10,0,0,0q�5      $��Center=�5      
�������$��电子雷管��5      �� 5   
   $��	0,0,100,0q�5      $��Right=�5      $��Center=�5      5(      �� 5)   
   $��Right=�5*      $��Center=�5+       �� Orientation$ 
Horizontal=�5,      .+�5-      �� -   5         ��Name$ MinimizeButton��5.      
�������$	��—��50         Triggers 51      +  %Microsoft.Xaml.Behaviors.EventTrigger  52         	EventName$ MouseLeftButtonDown��6%   /  )Microsoft.Xaml.Behaviors.EventTriggerBase   
SourceName$ MinimizeButton��6E   *  $Microsoft.Xaml.Behaviors.TriggerBase   Actions. 53      2  ,Microsoft.Xaml.Behaviors.InvokeCommandAction     Command 60   �� *
MinCommand+	 �� UpdateSourceTrigger$	 PropertyChanged=�54      56      �� 57      $��	15,0,15,0q�58      
�������$	��☐��5:       5;        5<      $ MouseLeftButtonDown��6%   . 5=         60   �� *
MaxCommand+
 �� Mode$
 OneWay=�5>      5@      �� 5A      $��0,0,15,0q�5B      
�������$	��✕��5D       5E        5F      $ MouseLeftButtonDown��6%   . 5G         60   �� *CloseCommand+$	 PropertyChanged=�5H      5J      5K      5L      5M      