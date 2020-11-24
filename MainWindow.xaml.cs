using System;
using System.Threading;
using System.Windows;

namespace UI
{
    public partial class MainWindow : Window
    {
        Thread thread_RefreshPanelContent;
        Thread thread_SimulateBuffersTraffic;
        Model.BufferFifo fifo_buffer;
        Model.BufferLifo lifo_buffer;
        Model.FiniteStateMachine fsm;
        bool stay_in_loop = true;

        public MainWindow()
        {
            InitializeComponent();
            fifo_buffer = Model.Model.CreateFifoBuffer();
            lifo_buffer = Model.Model.CreateLifoBuffer();
            fsm = Model.Model.InitialiseFsm();
            thread_RefreshPanelContent = new Thread(new ThreadStart(RefreshPanelContent));
            thread_SimulateBuffersTraffic = new Thread(new ThreadStart(SimulateBuffersTraffic));
            thread_RefreshPanelContent.Start();
            thread_SimulateBuffersTraffic.Start();
        }

        private void Button_fifo_write (object sender, RoutedEventArgs e)
        {
            if (fifo_write_data.Text != "")
            {
                byte data = byte.Parse(fifo_write_data.Text);
                stay_in_loop = fifo_buffer.write(data);
            }
        }
        private void Button_fifo_read (object sender, RoutedEventArgs e)
        {
            byte data = 0;
            stay_in_loop = fifo_buffer.read(ref data);
            fifo_read_data.Text = data.ToString();
        }
        private void Button_fifo_reset (object sender, RoutedEventArgs e)
        {
            fifo_buffer.reset();
        }

        private void Button_lifo_write (object sender, RoutedEventArgs e)
        {
            if (lifo_write_data.Text != "")
            {
                byte data = byte.Parse(lifo_write_data.Text);
                stay_in_loop = lifo_buffer.write(data);
            }
        }
        private void Button_lifo_read (object sender, RoutedEventArgs e)
        {
            byte data = 0;
            stay_in_loop = lifo_buffer.read(ref data);
            lifo_read_data.Text = data.ToString();
        }
        private void Button_lifo_reset (object sender, RoutedEventArgs e)
        {
            lifo_buffer.reset();
        }

        private void Button_fsm_card (object sender, RoutedEventArgs e)
        {
            fsm.HandlerInsertCard();
        }
        private void Button_fsm_pin (object sender, RoutedEventArgs e)
        {
            fsm.HandlerEnterPin();
        }
        private void Button_fsm_withdraw (object sender, RoutedEventArgs e)
        {
            fsm.HandlerWithdraw();
        }
        private void Button_fsm_terminate (object sender, RoutedEventArgs e)
        {
            fsm.HandlerTerminate();
        }
        private void Button_fsm_cancel (object sender, RoutedEventArgs e)
        {
            fsm.HandlerCancel();
        }

        private void SimulateBuffersTraffic()
        {
            Random rnd = new Random();
            byte data;

            while (true)
            {
                Thread.Sleep(2000);

                Dispatcher.Invoke(() =>
                {
                    data = (byte)rnd.Next(0, 255);

                    if ((data % 2) == 0)
                    {
                        if (fifo_buffer.write(data))
                        {
                            fifo_write_data.Text = data.ToString();
                        }
                        else
                        {
                            fifo_buffer.reset();
                        }

                        if (lifo_buffer.write(data))
                        {
                            lifo_write_data.Text = data.ToString();
                        }
                        else
                        {
                            lifo_buffer.reset();
                        }
                    }
                    else
                    {
                        if (fifo_buffer.read(ref data))
                        {
                            fifo_read_data.Text = data.ToString();
                        }
                        else
                        {
                            fifo_buffer.reset();
                        }

                        if (lifo_buffer.read(ref data))
                        {
                            lifo_read_data.Text = data.ToString();
                        }
                        else
                        {
                            lifo_buffer.reset();
                        }
                    }
                });
            }
        }

        private void RefreshPanelContent()
        {
            while (stay_in_loop)
            {
                Thread.Sleep(1000);

                Dispatcher.Invoke(() =>
                {
                    fifo_size.Text = fifo_buffer.get_size().ToString();
                    fifo_count.Text = fifo_buffer.get_count().ToString();
                    fifo_head.Text = fifo_buffer.get_head().ToString();
                    fifo_tail.Text = fifo_buffer.get_tail().ToString();
                    fifo_data.Text = fifo_buffer.get_buffer_string().ToString();

                    lifo_size.Text = lifo_buffer.get_size().ToString();
                    lifo_count.Text = lifo_head.Text = lifo_buffer.get_count().ToString();
                    lifo_data.Text = lifo_buffer.get_buffer_string().ToString();

                    fsm_state.Text = fsm.get_fsm_state().ToString();
                });
            }
        }
    }
}
