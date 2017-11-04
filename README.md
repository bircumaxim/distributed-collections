# Processing of distributed semistructured data
###### Here I added ability to proces distributed semistructured data. Each server cheeps data in Json, Xml, or Binary. Mediator should collect data and agregate it, convert to a format requested by the client again Xml, Json or Binary and sedn it the the Client.

#### In order to check how the system works blease refer to [readme ](https://github.com/bircumaxim/distributed-collections/blob/master/README.md) from master branch.

### Configuratoin
- In order to specify data type for each node you should simply add one more attribute in `DistributedSystem` config file. 

Exaple: 

```xml
<System>
    <Node Name="Node1" UdpIpEndPoint="127.0.0.1:3001" TcpIpEndPoint="127.0.0.1:5001" DataType="Xml"/>

    <Node Name="Node2" UdpIpEndPoint="127.0.0.1:3002" TcpIpEndPoint="127.0.0.1:5002" DataType="Json">
        <Node Name="Node1"/>
    </Node>

    <Node Name="Node3" UdpIpEndPoint="127.0.0.1:3003" TcpIpEndPoint="127.0.0.1:5003">
        <Node Name="Node5"/>
    </Node>

    <Node Name="Node4" UdpIpEndPoint="127.0.0.1:3004" TcpIpEndPoint="127.0.0.1:5004" DataType="Xml">
        <Node Name="Node6"/>
        <Node Name="Node5"/>
        <Node Name="Node2"/>
    </Node>

    <Node Name="Node5" UdpIpEndPoint="127.0.0.1:3005" TcpIpEndPoint="127.0.0.1:5005" DataType="Binary">
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

- In order to request data in a specific format from the `Mediator` you simply add one more option to `DataRequestMessageBuilder` like shown in example bellow in which we request `Json`.

Example:
```c#
 var requestMessage = new DataRequestMessageBuilder()
                    .OrderBy(empl => empl.Age)
                    .WithTimeout(2000)
                    .DataType(DataType.Json)
                    .Build();
```
