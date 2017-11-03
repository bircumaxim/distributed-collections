# MessageBroker
###### Simple distributed-collections system or build for educational purposes only. You could check bellow a short description and a guid of how to configure and to get started fast.
<p align="center">
  <img  src="https://github.com/bircumaxim/distributed-collections/blob/master/doc/system-diagram.png">
</p>


### Building
To buid `DistributedSystem` you firstly should download and build MessageBroker.
To build MessageBroker just folow steps below:
- open `MessageBroker.sln` in Visual Studio/Rider.
- set the working directory to be `MessageBroker` directory
- select as target project `MessageBroker`
- hit the run button 

To build `DistributedSystem` just folow steps below:
- open `DistributedSystem.sln` in Visual Studio/Rider.
- set the working directory to be `src` directory
- select as target project `DistributedSystem`
- hit the run button 

You'll find the built assemblies in /binaries.

If you see the build failing, check that you haven't put the source of MessageBroker in a deep subdirectory since long path names (greater than 248 characters) aren't supported by MSBuild.

### Configuration
#### MessageBroker
Firstly we should configure `MessageBroker` I'll leave an example of config gile for broker bellow.

We will need to add some connection managers: 
- `TcpConnectionManager` - receiving DiscoveryRequestMessages from `Mediator` 
- `UdpConnectionManager` - receiving DiscoveryResponseMessages from `Server Nodes` 
- `UdpMulticastConnectionManager` - sending `DicvoeryRequestMessage` to `Server Nodes`.

Now let's configure exchanges and queues: 
We will need just a Direct exchnage with 2 queues listed below:
- `discovery-requests`- for keeping discovery requests
- `discovery-responses`- for keeping discovery responses

##### Example
```xml
<?xml version="1.0" encoding="utf-8"?>`
<Broker>
    <Persistence>
        <FilePersistence EnableCrypting="true"/>
    </Persistence>

    <ConnectionManagers>
        <TcpConnectionManager />
        <UdpConnectionManager Port="8000"/>
        <UdpMulticastConnectionManager Ip="224.5.6.7" Port="7000">
            <Queue Name="discovery-requests"/>
        </UdpMulticastConnectionManager>
    </ConnectionManagers>

    <Exchanges>
        <DirectExchange Name="Discovery">
            <Queue Name="discovery-requests"/>
            <Queue Name="discovery-responses"/>
        </DirectExchange>
    </Exchanges>
</Broker>
```

#### Mediator
In order to comunicate with distributed system we should also configure mediator server.

We will need to add 2 Broker
- `Discovery` - for sending DicvoeryRequestMessage
- `DiscoveryResponse` - receiving DiscoveryResponseMessages

##### Example
```xml
<?xml version="1.0" encoding="utf-8"?>
<Buss>
    <Brokers>
        <Broker Name="Discovery" Ip="127.0.0.1" WireProtocol="DefaultWireProtocol">
            <Direct Name="Discovery"/>
        </Broker>

        <Broker Name="DiscoveryResponse" 
                SocketProtocol="Udp" 
                Ip="127.0.0.1" 
                Port="8000"
                ReceiverIp="127.0.0.1"
                ReceiverPort="5000"
                WireProtocol="DefaultWireProtocol"/>
    </Brokers>
</Buss>
```


#### DistributedSystem
Now let's configure and set up the graph of nodes

We will add 6 server nodes and will connect them as in diagram bellow.
<p align="center">
  <img  width="500"src="https://github.com/bircumaxim/distributed-collections/blob/master/doc/example-graph.png">
</p>


##### Example
```xml
<?xml version="1.0" encoding="utf-8"?>`
<System>
    <Node Name="Node1" UdpIpEndPoint="127.0.0.1:3001" TcpIpEndPoint="127.0.0.1:5001"/>

    <Node Name="Node2" UdpIpEndPoint="127.0.0.1:3002" TcpIpEndPoint="127.0.0.1:5002">
        <Node Name="Node4"/>
    </Node>

    <Node Name="Node3" UdpIpEndPoint="127.0.0.1:3003" TcpIpEndPoint="127.0.0.1:5003">
        <Node Name="Node5"/>
    </Node>

    <Node Name="Node4" UdpIpEndPoint="127.0.0.1:3004" TcpIpEndPoint="127.0.0.1:5004">
        <Node Name="Node6"/>
        <Node Name="Node5"/>
        <Node Name="Node2"/>
    </Node>

    <Node Name="Node5" UdpIpEndPoint="127.0.0.1:3005" TcpIpEndPoint="127.0.0.1:5005">
        <Node Name="Node6"/>
        <Node Name="Node4"/>
        <Node Name="Node3"/>
    </Node>

    <Node Name="Node6" UdpIpEndPoint="127.0.0.1:3006" TcpIpEndPoint="127.0.0.1:5006">
        <Node Name="Node4"/>
        <Node Name="Node5"/>
    </Node>
</System>
```

