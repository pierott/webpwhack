# webpwhack
Converts webp files into jpeg once they're copied into a folder.

In the first version I tried to make UI as simple as possible.
You can select a folder and decide if you want to start with the system.

The app starts looking into folder only when you press "Start Running" button.
Then it'll stay active even if you close the window. If you want to see the window again, just run the exe.
It'll notify the active process that you want to configure it so it'll show the window.

If it's not in the active state, closing the window will stop the process because there's no point in running it.

When it autostarts it won't show the window. Again, if you want to configure it or stop, just launch exe.
Hopefully, it'll become more intuitive when you try it.
