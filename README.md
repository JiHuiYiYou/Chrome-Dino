# 🦖 2D 无尽跑酷游戏 (Dino Clone)

这是一个使用 **Godot Engine 4.x (C#)** 开发的 2D 无尽跑酷游戏原型。  
本项目旨在复现经典浏览器游戏（如 Chrome Dino）的核心机制，并探索 Godot 中视差背景的高级控制技巧。

## ✨ 核心特性

### 🌌 智能视差背景系统
- **多层级独立控制**：支持任意数量的 `Parallax2D` 层。
- **动态加速**：背景速度随时间推移逐渐加快，模拟游戏难度提升。
- **数学驱动**：基于原始 `Autoscroll` 速度，通过系数自动计算加速度与速度上限，保证各层视觉协调性。
- **硬上限限制**：速度在达到设定阈值后恒定，防止因无限加速导致的物理崩溃。

### 🎮 玩家控制
- **静态角色**：玩家角色在屏幕 X 轴位置固定（仅进行跳跃动作）。
- **物理驱动**：基于 `CharacterBody2D` 的重力和跳跃逻辑。

### 🏗️ 技术架构
- **语言**：C#
- **引擎**：Godot 4.x
- **设计模式**：数据驱动、节点解耦

## 🛠️ 技术实现细节

### Parallax 加速算法
不同于传统的直接设置速度，本项目采用了基于导数的增量控制：

csharp
// 伪代码逻辑
currentSpeed = min(currentSpeed + acceleration * delta, maxSpeed);
layer.Autoscroll = new Vector2(currentSpeed, 0);


- **Acceleration**：基于原始速度的缩放系数，确保不同层加速节奏一致。
- **MaxSpeed**：同样基于原始速度的比例，保证视觉层次感。

### 节点结构

- World (Node2D)
- ├── ParallaxRoot (Node2D)
- │   ├── Parallax2D (远景)
- │   │   ├── Sprite2D
- │   │   └── ParallaxLayerConfig (Node)
- │   ├── Parallax2D2 (中景)
- │   │   ├── Sprite2D
- │   │   └── ParallaxLayerConfig
- │   └── ...
- ├── Player (CharacterBody2D)
- │   ├── Sprite2D
- │   └── AnimationPlayer
- └── WorldManager (Node)


## 🚀 如何运行

1. **克隆仓库**
   bash
   git clone https://github.com/your-username/your-repo-name.git

2. **打开 Godot**
   - 选择“导入”项目。
   - 指向项目文件夹中的 `project.godot` 文件。
3. **运行**
   - 点击 Godot 编辑器的“播放”按钮（F5）。

## 📅 开发路线图

- [x] ✅ 基础玩家移动（跳跃）
- [x] ✅ Parallax 背景动态加速
- [ ] 🔲 玩家动画系统（Idle/Run/Jump）
- [ ] 🔲 障碍物生成系统
- [ ] 🔲 碰撞检测与游戏结束逻辑
- [ ] 🔲 UI（分数、倒计时）
- [ ] 🔲 音效与粒子效果

## 📝 日志

- **Day 1**: 攻克了 `Parallax2D` 的 `Autoscroll` 属性在 C# 中的正确调用方式，解决了 CS1612 及无限加速 Bug，确立了稳定的视差加速模型。

---
*Made with ❤️ and lots of debugging.*