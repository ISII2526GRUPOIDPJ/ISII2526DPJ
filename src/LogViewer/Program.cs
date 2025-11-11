namespace LogViewer;

class Program
{
    static void Main(string[] args) {
        Subscriber subscriber = new Subscriber();

        try {
            subscriber.StartConsuming();
        }
        finally {
            subscriber.DisposeResources();
        }
    }
}