# White Noise (UWP Application)

## **Overview**
The **"White Noise"** UWP application is a graphical program that generates and displays white noise as an image.

![White Noise Animation](white-noise.gif)

## **Functionality**
### **1. Canvas-based Rendering**
- The application uses a `CanvasControl` (from Win2D) to render graphics.
- It dynamically updates the canvas at a rate of **every 10 milliseconds** (100 FPS max).

### **2. White Noise Generation**
- It creates a **random black-and-white pixel pattern** by setting each pixel's value randomly to either **black (0,0,0,255)** or **white (255,255,255,255)**.
- The pixels are stored in a `SoftwareBitmap` and then copied into a `CanvasBitmap` for rendering.

### **3. Frame Rate Calculation**
- The application tracks the number of frames rendered per second.
- Every second, it updates a UI element (`FPS.Text`) with the **current FPS**.

### **4. Memory Management**
- The program uses **direct memory access** (via `IMemoryBufferByteAccess`) to efficiently manipulate pixel data.
- This ensures fast updates for each new frame.

### **5. Timer-based Updates**
- The canvas is redrawn at regular intervals (every 10 ms) using a `System.Timers.Timer`.

## **Visual Effect**
- The app continuously **generates and displays random noise**, giving the effect of an old **TV static screen**.
- The FPS counter ensures the application runs smoothly and efficiently.

## **Possible Improvements**
- Instead of a **fixed interval of 10ms**, dynamically adjust the timer to match the display refresh rate.
- Use **Compute Shaders or GPU Acceleration** for even better performance.
- Provide user controls to **adjust noise density or colors**.
