namespace Model
{
    public enum FsmState
    {
        STATE_Idle,
        STATE_Card_Inserted,
        STATE_Pin_Entered,
        STATE_Money_Ready,
        STATE_INVALID
    }

    public enum FsmEvent
    {
        EVENT_Card_Insert,
        EVENT_Pin_Enter,
        EVENT_Withdraw,
        EVENT_Terminate,
        EVENT_Cancel,
        EVENT_INVALID
    }

    public class Model
    {
        public static BufferFifo CreateFifoBuffer()
        {
            return new BufferFifo();
        }
        public static BufferLifo CreateLifoBuffer()
        {
            return new BufferLifo();
        }
        public static FiniteStateMachine InitialiseFsm()
        {
            return new FiniteStateMachine();
        }
    }

    public class BufferFifo
    {
        private const int BUFFER_SIZE = 20;
        private byte[] BUFFER_DATA = new byte[BUFFER_SIZE] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        private int count = 0;
        private int head = 0;
        private int tail = 0;

        public bool write(byte data)
        {
            bool exit_code = false;

            if (count != BUFFER_SIZE)
            {
                count++;
                BUFFER_DATA[head] = data;
                head = (head + 1) % BUFFER_SIZE;
                exit_code = true;
            }

            return exit_code;
        }
        public bool read(ref byte data)
        {
            bool exit_code = false;

            if (count != 0)
            {
                count--;
                data = BUFFER_DATA[tail];
                tail = (tail + 1) % BUFFER_SIZE;
                exit_code = true;
            }

            return exit_code;
        }
        public void reset()
        {
            count = 0;
            head = 0;
            tail = 0;
        }
        public int get_size()
        {
            return BUFFER_SIZE;
        }
        public int get_count()
        {
            return count;
        }
        public int get_head()
        {
            return head;
        }
        public int get_tail()
        {
            return tail;
        }
        public string get_buffer_string()
        {
            string buffer_string = "";

            for (int i = 0; i < BUFFER_SIZE; i++)
            {
                if (count == 0)
                {
                    buffer_string += "* ";
                }
                else if (tail < head)
                {
                    if ((i >= tail) && (i < head))
                    {
                        buffer_string = buffer_string + BUFFER_DATA[i].ToString() + " ";
                    }
                    else
                    {
                        buffer_string += "* ";
                    }
                }
                else
                {
                    if ((i <= head) || (i > tail))
                    {
                        buffer_string = buffer_string + BUFFER_DATA[i].ToString() + " ";
                    }
                    else
                    {
                        buffer_string += "* ";
                    }
                }
            }

            return buffer_string;
        }
    }

    public class BufferLifo
    {
        private const int BUFFER_SIZE = 30;
        private byte[] BUFFER_DATA = new byte[BUFFER_SIZE] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        private int count = 0;

        public bool write (byte data)
        {
            bool exit_code = false;

            if (count != BUFFER_SIZE)
            {
                BUFFER_DATA[count] = data;
                count++;
                exit_code = true;
            }

            return exit_code;
        }
        public bool read (ref byte data)
        {
            bool exit_code = false;

            if (count != 0)
            {
                count--;
                data = BUFFER_DATA[count];
                exit_code = true;
            }

            return exit_code;
        }
        public void reset()
        {
            count = 0;
        }
        public int get_size()
        {
            return BUFFER_SIZE;
        }
        public int get_count()
        {
            return count;
        }
        public string get_buffer_string()
        {
            string buffer_string = "";

            for (int i = 0; i < BUFFER_SIZE; i++)
            {
                if (i < count)
                {
                    buffer_string = buffer_string + BUFFER_DATA[i].ToString() + " ";
                }
                else
                {
                    buffer_string += "* ";
                }
            }
            return buffer_string;
        }
    }

    public class FiniteStateMachine
    {
        private FsmState fsm_state = FsmState.STATE_Idle;

        public void HandlerInsertCard()
        {
            if (fsm_state == FsmState.STATE_Idle)
            {
                fsm_state = FsmState.STATE_Card_Inserted;
            }
        }
        public void HandlerEnterPin()
        {
            if (fsm_state == FsmState.STATE_Card_Inserted)
            {
                fsm_state = FsmState.STATE_Pin_Entered;
            }
        }
        public void HandlerWithdraw()
        {
            if (fsm_state == FsmState.STATE_Pin_Entered)
            {
                fsm_state = FsmState.STATE_Money_Ready;
            }
        }
        public void HandlerTerminate()
        {
            if (fsm_state == FsmState.STATE_Money_Ready)
            {
                fsm_state = FsmState.STATE_Idle;
            }
        }
        public void HandlerCancel()
        {
            fsm_state = FsmState.STATE_Idle;
        }
        public FsmState get_fsm_state()
        {
            return fsm_state;
        }
    }
}
