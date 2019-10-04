代码分为7个文件：
Model：负责导演类与相关接口
Controller：负责调控整个游戏的运行
View：负责游戏界面的文本、IO操作
Action：和上一次的作业一样，把Controller当中的动作（飞碟运动）提取出来放在了这个类中
DiskFactory：按照这次的要求，使用工厂模式来生产、回收飞碟（因此Model类的工作减少了）
ScoreRecorder：负责记录分数
DiskData：方便不同飞碟的预制参数调整

场景为hit ufo/Assets/Scenes/SampleScene.unity