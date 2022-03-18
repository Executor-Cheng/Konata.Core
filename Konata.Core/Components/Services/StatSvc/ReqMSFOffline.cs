﻿using Konata.Core.Attributes;
using Konata.Core.Common;
using Konata.Core.Events.Model;
using Konata.Core.Packets;
using Konata.Core.Packets.SvcPush;

// ReSharper disable InconsistentNaming
// ReSharper disable UnusedType.Global

namespace Konata.Core.Components.Services.StatSvc;

[EventSubscribe(typeof(ReqMSFOfflineEvent))]
[Service("StatSvc.ReqMSFOffline", PacketType.TypeB, AuthFlag.D2Authentication, SequenceMode.Managed)]
internal class ReqMSFOffline : BaseService<ReqMSFOfflineEvent>
{
    protected override bool Parse(SSOFrame input,
        BotKeyStore keystore, out ReqMSFOfflineEvent output)
    {
        var tree = new SvcPushMsfForceOffline(input.Payload.GetBytes());
        output = new ReqMSFOfflineEvent(tree.Title, tree.Message);
        return true;
    }
}