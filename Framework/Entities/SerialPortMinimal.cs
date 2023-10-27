using System.IO.Ports;

namespace PatitoClient.Framework.Entities
{
    internal class SerialPortMinimal : SerialPort
    {
        private SerialPortMinimal( ) {}

        public class Builder
        {
            private SerialPortMinimal _serialPortMinimal;

            public Builder()
            {
                _serialPortMinimal = new SerialPortMinimal();
            }

            public SerialPortMinimal Build()
            {
                return _serialPortMinimal;
            }

            public Builder AddPortName(string portName)
            {
                _serialPortMinimal.PortName = portName;
                return this;
            }

            public Builder AddBaudRate(int baudRate)
            {
                _serialPortMinimal.BaudRate = baudRate;
                return this;
            }

            public Builder AddParity(Parity parity)
            {
                _serialPortMinimal.Parity = parity;
                return this;
            }

            public Builder AddDataBits(int dataBits)
            {
                _serialPortMinimal.DataBits = dataBits;
                return this;
            }

            public Builder AddStopBits(StopBits stopBits)
            {

                _serialPortMinimal.StopBits = stopBits;
                return this;
            }

            public Builder AddReceivedBytesThreshold(int receivedBytesThreshold)
            {
                _serialPortMinimal.ReceivedBytesThreshold = receivedBytesThreshold;
                return this;
            }

            public Builder AddDataReceived(SerialDataReceivedEventHandler dataReceived)
            {
                _serialPortMinimal.DataReceived += dataReceived;
                return this;
            }
        }
    }
}
