This is the full script for a basic drawing functionality using Unity and the UI RawImage component. The script creates a new texture based on an original texture and displays it on a RawImage component. It allows the user to draw on the image using the mouse cursor.

The script defines several variables, including the RawImage component, a scaling factor, a Texture2D object for the canvas, an array of colors to represent the canvas, and the previous position of the mouse cursor. It also includes two public variables to define the brush size and color.

The script first calculates the scaling factor based on the size of the RawImage and the original texture. It then creates a new Texture2D object and copies the pixels from the original texture to the new texture using a double for loop. The canvas texture is set as the new texture, and the RawImage is set to display the canvas texture.

The Update function is called every frame and checks whether the left mouse button is being pressed. If so, it calls the DrawLineTo function with the current mouse cursor position. If not, it resets the previous position to (0,0).

The DrawLineTo function takes the current position of the mouse cursor and draws a line from the previous position to the current position. It calculates the number of steps needed to draw the line based on the brush size and distance between the previous and current position. It then uses a double for loop to draw a circle at each point along the line using the DrawPixel function. Finally, it updates the canvas texture and sets the previous position to the current position.

The DrawPixel function takes an x and y position, a color, and a size as arguments. It draws a circle with the specified size and color at the given position on the canvas. It loops through each pixel in a square area around the given position and checks whether it is within the circle. If so, it sets the color of that pixel to the brush color.

The GetLocalCursor function takes the current mouse position and converts it to a local position relative to the RawImage component. It then adds an offset to center the cursor within the canvas.
