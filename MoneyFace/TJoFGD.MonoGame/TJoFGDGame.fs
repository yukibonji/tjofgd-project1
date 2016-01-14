﻿module TJoFGDGame

open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Graphics
open Microsoft.Xna.Framework.Content

type TJoFGDGame<'assetKey,'assetValue when 'assetKey: comparison> 
        (backBufferWidth, backBufferHeight, assetLoader: ContentManager -> Map<'assetKey, 'assetValue>, gameUpdater, gameRenderer : GameTime -> Map<'assetKey,'assetValue> -> SpriteBatch -> unit, backgroundColor) as this=
    inherit Game()

    do
        this.Content.RootDirectory <- "Content"

    let graphics = new GraphicsDeviceManager(this)

    let mutable spriteBatch: SpriteBatch = null
    let mutable assets: Map<'assetKey, 'assetValue> = Map.empty

    override this.Initialize() =
        graphics.PreferredBackBufferWidth <- backBufferWidth
        graphics.PreferredBackBufferHeight <- backBufferHeight
        graphics.ApplyChanges()
        spriteBatch <- new SpriteBatch(this.GraphicsDevice)
        base.Initialize()

    override this.LoadContent() = 
        assets <- this.Content |> assetLoader 

    override this.Update delta =
        delta
        |> gameUpdater 

    override this.Draw delta =
        backgroundColor 
        |> this.GraphicsDevice.Clear
        spriteBatch.Begin()
        gameRenderer delta assets spriteBatch
        spriteBatch.End()


