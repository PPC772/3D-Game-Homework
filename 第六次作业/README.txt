代码分为10个文件：
Model：负责导演类与相关接口
Controller：负责调控整个游戏的运行
View：负责游戏界面的文本、IO操作
DiskFactory：按照这次的要求，使用工厂模式来生产、回收飞碟（因此Model类的工作减少了）
ScoreRecorder：负责记录分数
DiskData：方便不同飞碟的预制参数调整
ActionInterface：运动学/动力学动作管理器的共用代码部分
ActionManagerAdapter：提供两个动作管理器的切换
DynamicsAction：动力学动作管理器（通过给飞碟施加作用力来模拟飞碟作抛物线运动）
KinematicsAction：运动学动作管理器（通过改变飞碟坐标来模拟飞碟作抛物线运动）

在main camera中的Controller脚本里有个switch_phy的复选框，在两个动作管理器之间进行切换。

场景为hit ufo/Assets/Scenes/SampleScene.unity