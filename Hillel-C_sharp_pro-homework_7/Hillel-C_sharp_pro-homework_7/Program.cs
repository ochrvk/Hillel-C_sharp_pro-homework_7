class BarberShop
{
    static SemaphoreSlim barberSemaphore = new SemaphoreSlim(0);
    static SemaphoreSlim customerSemaphore = new SemaphoreSlim(0); 
    static object chairLock = new object();
    static int availableChairs = 5; 

    static void Main()
    {
        Thread barberThread = new Thread(Barber);
        barberThread.Start();

        for (int i = 1; i <= 10; i++)
        {
            Thread customerThread = new Thread(Customer);
            customerThread.Start(i);
            Thread.Sleep(1000);
        }

        Console.ReadLine();
    }

    static void Barber()
    {
        while (true)
        {
            Console.WriteLine("Barber is sleeping...");

            customerSemaphore.Wait(); 
            lock (chairLock)
            {
                availableChairs++;
            }

            Console.WriteLine("Barber is cutting hair");

            Thread.Sleep(3000); 

            barberSemaphore.Release();
        }
    }

    static void Customer(object customerId)
    {
        int id = (int)customerId;
        bool gotHaircut = false;

        lock (chairLock)
        {
            if (availableChairs > 0)
            {
                availableChairs--;

                Console.WriteLine($"Customer {id} is waiting in the waiting room.");

                customerSemaphore.Release();
                gotHaircut = true;
            }
            else
            {
                Console.WriteLine($"Customer {id} left because there are no available chairs.");
            }
        }

        if (gotHaircut)
        {
            barberSemaphore.Wait();

            Console.WriteLine($"Customer {id} got a haircut and leaves the barber shop.");
        }
    }
}



