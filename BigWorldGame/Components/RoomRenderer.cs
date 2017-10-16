﻿using System.Threading;
using BigWorld.World;
using engenious;
using engenious.Graphics;

namespace BigWorldGame.Components
{
    public class RoomRenderer : DrawableGameComponent
    {
        public new readonly MainGame Game;

        private SpriteBatch spriteBatch;
        private Texture2D pixeltexture;

        private Texture2D dungeonSheet;
        
        private Point tilePoint = new Point(9,2);
        
        public RoomRenderer(MainGame game) : base(game)
        {
            this.Game = game;
        }


        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            
            pixeltexture = new Texture2D(GraphicsDevice,1,1);
            pixeltexture.SetData(new Color[] {Color.White});

            dungeonSheet = Game.Content.Load<Texture2D>("Spritesheets/TileSheetDungeon");
        }

        public override void Draw(GameTime gameTime)
        {   
            spriteBatch.Begin();

            for (int x = -1; x < 2; x++)
            {
                for (int y = -1; y < 2; y++)
                {
                    DrawRoom(new Point(x,y));
                }  
            }
            
            spriteBatch.End();
        }

        private void DrawRoom(Point point)
        {
            var height = (GraphicsDevice.Viewport.Height - Room.RoomSizeY * RenderSettings.TileSize)/2;
            
            var size = new Point(RenderSettings.TileSize,RenderSettings.TileSize);
            
            var delta = new Point(size.X * Room.RoomSizeX* point.X,size.Y * Room.RoomSizeY* point.Y);

            float alpha = point.IsEmpty ? 1f : 0.5f; 
            
            var realTilePoint = new Point(17*tilePoint.X,17* tilePoint.Y);
            
            for (int x = 0; x < Room.RoomSizeX; x++)
            {
                for (int y = 0; y < Room.RoomSizeY; y++)
                {
                    var drawPoint = new Point(x*RenderSettings.TileSize+height,y*RenderSettings.TileSize+height) + delta;
                    
                    
                    spriteBatch.Draw(dungeonSheet,new Rectangle(drawPoint,size),new Rectangle(realTilePoint,new Point(16,16)),Color.White * alpha );
                }
            }
        }
    }
}