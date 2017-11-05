﻿using BigWorld;
using BigWorld.GUI;
using BigWorld.Map;
using BigWorldGame.Components;
using BigWorldGame.GUI;
using engenious;
using engenious.Graphics;
using engenious.Input;

namespace BigWorldGame
{
    public class MainGame : Game
    {
        
        public int CurrentLayer = -1;
        
        public GameState CurrentGameState = GameState.Build;
                
        public WorldMap BuildWorldMap = new WorldMap();                
        
        private readonly Trigger<bool> debugTrigger = new Trigger<bool>();
        private readonly Trigger<bool> buildTrigger = new Trigger<bool>();
        private readonly Trigger<bool> runTrigger = new Trigger<bool>();
        
        private readonly Trigger<bool> saveTrigger = new Trigger<bool>();
        private readonly Trigger<bool> loadTrigger = new Trigger<bool>();
        
        public readonly MapRenderer MapRenderer;
        public readonly BuildMapOverlayRenderer BuildMapOverlayRenderer;
        public readonly GuiRenderer GuiRenderer;
        public readonly SimulationComponent SimulationComponent;
        public readonly EntityRenderer EntityRenderer;

        public readonly CameraComponet CameraComponet;

        private TileSelectControl tileSelectControl;

        public MainGame()
        {
            CameraComponet = new CameraComponet(this);
            Components.Add(CameraComponet);

            MapRenderer = new MapRenderer(this);
            Components.Add(MapRenderer);

            BuildMapOverlayRenderer = new BuildMapOverlayRenderer(this);
            Components.Add(BuildMapOverlayRenderer);
            
            GuiRenderer = new GuiRenderer(this);
            Components.Add(GuiRenderer);
            
            SimulationComponent = new SimulationComponent(this);
            Components.Add(SimulationComponent);
            
            EntityRenderer = new EntityRenderer(this);
            Components.Add(EntityRenderer);

            var scrollContainer = new ScrollContainer();
            //scrollContainer.HorizontalAlignment = BigWorld.GUI.Layout.HorizontalAlignment.Stretch;
            //scrollContainer.VerticalAlignment = BigWorld.GUI.Layout.VerticalAlignment.Stretch;
            GuiRenderer.RootControl.Content = scrollContainer;

            tileSelectControl = new TileSelectControl();
            scrollContainer.Content = tileSelectControl;
        }

        public override void LoadContent()
        {
            base.LoadContent();

            tileSelectControl.SpriteSheet = new Graphics.Spritesheet(GraphicsDevice, Content, "Spritesheets/TileSheetDungeon", 16, 16);
        }

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            GraphicsDevice.Viewport = new Viewport(0,0,Window.ClientSize.Height,Window.ClientSize.Height);
            CameraComponet.Draw(gameTime);
            MapRenderer.Draw(gameTime);
            BuildMapOverlayRenderer.Draw(gameTime);
            
            EntityRenderer.Draw(gameTime);
            
            GraphicsDevice.Viewport = new Viewport(0,0,Window.ClientSize.Width,Window.ClientSize.Height);
            GuiRenderer.Draw(gameTime);          
        }

        public override void Update(GameTime gameTime)
        {
            var keyState = Keyboard.GetState();
            
            if (buildTrigger.IsChanged(keyState.IsKeyDown(Keys.F4), k => k) && CurrentGameState != GameState.Build)
            {
                CurrentGameState = GameState.Build;
                SimulationComponent.Reset(GameState.Build);
            }
            
            if (debugTrigger.IsChanged(keyState.IsKeyDown(Keys.F5), k => k) && CurrentGameState != GameState.Debug)
            {
                CurrentGameState = GameState.Debug;
                SimulationComponent.Reset(GameState.Debug);
            }

            if (runTrigger.IsChanged(keyState.IsKeyDown(Keys.F6),k => k) && CurrentGameState != GameState.Running)
            {
                CurrentGameState = GameState.Running;
                SimulationComponent.SimulationWorld = BuildWorldMap;
                SimulationComponent.Reset(GameState.Running);
            }

            if (saveTrigger.IsChanged(keyState.IsKeyDown(Keys.F11), k => k) && CurrentGameState == GameState.Build)
            {
                BuildWorldMap.SaveWorld();
            }
            
            if (loadTrigger.IsChanged(keyState.IsKeyDown(Keys.F12), k => k) && CurrentGameState == GameState.Build)
            {
                BuildWorldMap = WorldMap.LoadWorld();
            }
            
            base.Update(gameTime);
        }
    }
}