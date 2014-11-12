using System;
using System.Reflection;

namespace Grapevine
{
    /// <summary>
    /// MIME type of the body of the request or response
    /// </summary>
    public enum ContentType
    {
        [ContentTypeMetadata(Value = "application/x-authorware-bin", IsBinary = true)]
        AAB,

        [ContentTypeMetadata(Value = "audio/x-aac", IsBinary = true)]
        AAC,

        [ContentTypeMetadata(Value = "application/x-authorware-map", IsBinary = true)]
        AAM,

        [ContentTypeMetadata(Value = "application/x-authorware-seg", IsBinary = true)]
        AAS,

        [ContentTypeMetadata(Value = "application/x-abiword", IsBinary = true)]
        ABW,

        [ContentTypeMetadata(Value = "application/pkix-attr-cert", IsBinary = true)]
        AC,

        [ContentTypeMetadata(Value = "application/vnd.americandynamics.acc", IsBinary = true)]
        ACC,

        [ContentTypeMetadata(Value = "application/x-ace-compressed", IsBinary = true)]
        ACE,

        [ContentTypeMetadata(Value = "application/vnd.acucobol", IsBinary = true)]
        ACU,

        [ContentTypeMetadata(Value = "application/vnd.acucorp", IsBinary = true)]
        ACUTC,

        [ContentTypeMetadata(Value = "audio/adpcm", IsBinary = true)]
        ADP,

        [ContentTypeMetadata(Value = "application/vnd.audiograph", IsBinary = true)]
        AEP,

        [ContentTypeMetadata(Value = "application/x-font-type1", IsBinary = true)]
        AFM,

        [ContentTypeMetadata(Value = "application/vnd.ibm.modcap", IsBinary = true)]
        AFP,

        [ContentTypeMetadata(Value = "application/vnd.ahead.space", IsBinary = true)]
        AHEAD,

        [ContentTypeMetadata(Value = "application/postscript", IsBinary = true)]
        AI,

        [ContentTypeMetadata(Value = "audio/x-aiff", IsBinary = true)]
        AIF,

        [ContentTypeMetadata(Value = "audio/x-aiff", IsBinary = true)]
        AIFC,

        [ContentTypeMetadata(Value = "audio/x-aiff", IsBinary = true)]
        AIFF,

        [ContentTypeMetadata(Value = "application/vnd.adobe.air-application-installer-package+zip", IsBinary = true)]
        AIR,

        [ContentTypeMetadata(Value = "application/vnd.dvb.ait", IsBinary = true)]
        AIT,

        [ContentTypeMetadata(Value = "application/vnd.amiga.ami", IsBinary = true)]
        AMI,

        [ContentTypeMetadata(Value = "application/vnd.android.package-archive", IsBinary = true)]
        APK,

        [ContentTypeMetadata(Value = "text/cache-manifest", IsText = true)]
        APPCACHE,

        [ContentTypeMetadata(Value = "application/x-ms-application", IsBinary = true)]
        APPLICATION,

        [ContentTypeMetadata(Value = "application/vnd.lotus-approach", IsBinary = true)]
        APR,

        [ContentTypeMetadata(Value = "application/x-freearc", IsBinary = true)]
        ARC,

        [ContentTypeMetadata(Value = "text/plain", IsText = true)]
        ASC,

        [ContentTypeMetadata(Value = "video/x-ms-asf", IsBinary = true)]
        ASF,

        [ContentTypeMetadata(Value = "text/x-asm", IsText = true)]
        ASM,

        [ContentTypeMetadata(Value = "application/vnd.accpac.simply.aso", IsBinary = true)]
        ASO,

        [ContentTypeMetadata(Value = "video/x-ms-asf", IsBinary = true)]
        ASX,

        [ContentTypeMetadata(Value = "application/vnd.acucorp", IsBinary = true)]
        ATC,

        [ContentTypeMetadata(Value = "application/atom+xml", IsText = true)]
        ATOM,

        [ContentTypeMetadata(Value = "application/atomcat+xml", IsText = true)]
        ATOMCAT,

        [ContentTypeMetadata(Value = "application/atomsvc+xml", IsText = true)]
        ATOMSVC,

        [ContentTypeMetadata(Value = "application/vnd.antix.game-component", IsBinary = true)]
        ATX,

        [ContentTypeMetadata(Value = "audio/basic", IsBinary = true)]
        AU,

        [ContentTypeMetadata(Value = "video/x-msvideo", IsBinary = true)]
        AVI,

        [ContentTypeMetadata(Value = "application/applixware", IsBinary = true)]
        AW,

        [ContentTypeMetadata(Value = "application/vnd.airzip.filesecure.azf", IsBinary = true)]
        AZF,

        [ContentTypeMetadata(Value = "application/vnd.airzip.filesecure.azs", IsBinary = true)]
        AZS,

        [ContentTypeMetadata(Value = "application/vnd.amazon.ebook", IsBinary = true)]
        AZW,

        [ContentTypeMetadata(Value = "application/x-msdownload", IsBinary = true)]
        BAT,

        [ContentTypeMetadata(Value = "application/x-bcpio", IsBinary = true)]
        BCPIO,

        [ContentTypeMetadata(Value = "application/x-font-bdf", IsBinary = true)]
        BDF,

        [ContentTypeMetadata(Value = "application/vnd.syncml.dm+wbxml", IsText = true)]
        BDM,

        [ContentTypeMetadata(Value = "application/vnd.realvnc.bed", IsBinary = true)]
        BED,

        [ContentTypeMetadata(Value = "application/vnd.fujitsu.oasysprs", IsBinary = true)]
        BH2,

        [ContentTypeMetadata(Value = "application/octet-stream", IsBinary = true)]
        BIN,

        [ContentTypeMetadata(Value = "application/x-blorb", IsBinary = true)]
        BLB,

        [ContentTypeMetadata(Value = "application/x-blorb", IsBinary = true)]
        BLORB,

        [ContentTypeMetadata(Value = "application/vnd.bmi", IsBinary = true)]
        BMI,

        [ContentTypeMetadata(Value = "image/bmp", IsBinary = true)]
        BMP,

        [ContentTypeMetadata(Value = "application/vnd.framemaker", IsBinary = true)]
        BOOK,

        [ContentTypeMetadata(Value = "application/vnd.previewsystems.box", IsBinary = true)]
        BOX,

        [ContentTypeMetadata(Value = "application/x-bzip2", IsBinary = true)]
        BOZ,

        [ContentTypeMetadata(Value = "application/octet-stream", IsBinary = true)]
        BPK,

        [ContentTypeMetadata(Value = "image/prs.btif", IsBinary = true)]
        BTIF,

        [ContentTypeMetadata(Value = "application/x-bzip", IsBinary = true)]
        BZ,

        [ContentTypeMetadata(Value = "application/x-bzip2", IsBinary = true)]
        BZ2,

        [ContentTypeMetadata(Value = "text/x-c", IsText = true)]
        C,

        [ContentTypeMetadata(Value = "application/vnd.cluetrust.cartomobile-config", IsBinary = true)]
        C11AMC,

        [ContentTypeMetadata(Value = "application/vnd.cluetrust.cartomobile-config-pkg", IsBinary = true)]
        C11AMZ,

        [ContentTypeMetadata(Value = "application/vnd.clonk.c4group", IsBinary = true)]
        C4D,

        [ContentTypeMetadata(Value = "application/vnd.clonk.c4group", IsBinary = true)]
        C4F,

        [ContentTypeMetadata(Value = "application/vnd.clonk.c4group", IsBinary = true)]
        C4G,

        [ContentTypeMetadata(Value = "application/vnd.clonk.c4group", IsBinary = true)]
        C4P,

        [ContentTypeMetadata(Value = "application/vnd.clonk.c4group", IsBinary = true)]
        C4U,

        [ContentTypeMetadata(Value = "application/vnd.ms-cab-compressed", IsBinary = true)]
        CAB,

        [ContentTypeMetadata(Value = "audio/x-caf", IsBinary = true)]
        CAF,

        [ContentTypeMetadata(Value = "application/vnd.tcpdump.pcap", IsBinary = true)]
        CAP,

        [ContentTypeMetadata(Value = "application/vnd.curl.car", IsBinary = true)]
        CAR,

        [ContentTypeMetadata(Value = "application/vnd.ms-pki.seccat", IsBinary = true)]
        CAT,

        [ContentTypeMetadata(Value = "application/x-cbr", IsBinary = true)]
        CB7,

        [ContentTypeMetadata(Value = "application/x-cbr", IsBinary = true)]
        CBA,

        [ContentTypeMetadata(Value = "application/x-cbr", IsBinary = true)]
        CBR,

        [ContentTypeMetadata(Value = "application/x-cbr", IsBinary = true)]
        CBT,

        [ContentTypeMetadata(Value = "application/x-cbr", IsBinary = true)]
        CBZ,

        [ContentTypeMetadata(Value = "text/x-c", IsText = true)]
        CC,

        [ContentTypeMetadata(Value = "application/x-director", IsBinary = true)]
        CCT,

        [ContentTypeMetadata(Value = "application/ccxml+xml", IsText = true)]
        CCXML,

        [ContentTypeMetadata(Value = "application/vnd.contact.cmsg", IsBinary = true)]
        CDBCMSG,

        [ContentTypeMetadata(Value = "application/x-netcdf", IsBinary = true)]
        CDF,

        [ContentTypeMetadata(Value = "application/vnd.mediastation.cdkey", IsBinary = true)]
        CDKEY,

        [ContentTypeMetadata(Value = "application/cdmi-capability", IsBinary = true)]
        CDMIA,

        [ContentTypeMetadata(Value = "application/cdmi-container", IsBinary = true)]
        CDMIC,

        [ContentTypeMetadata(Value = "application/cdmi-domain", IsBinary = true)]
        CDMID,

        [ContentTypeMetadata(Value = "application/cdmi-object", IsBinary = true)]
        CDMIO,

        [ContentTypeMetadata(Value = "application/cdmi-queue", IsBinary = true)]
        CDMIQ,

        [ContentTypeMetadata(Value = "chemical/x-cdx", IsBinary = true)]
        CDX,

        [ContentTypeMetadata(Value = "application/vnd.chemdraw+xml", IsText = true)]
        CDXML,

        [ContentTypeMetadata(Value = "application/vnd.cinderella", IsBinary = true)]
        CDY,

        [ContentTypeMetadata(Value = "application/pkix-cert", IsBinary = true)]
        CER,

        [ContentTypeMetadata(Value = "application/x-cfs-compressed", IsBinary = true)]
        CFS,

        [ContentTypeMetadata(Value = "image/cgm", IsBinary = true)]
        CGM,

        [ContentTypeMetadata(Value = "application/x-chat", IsBinary = true)]
        CHAT,

        [ContentTypeMetadata(Value = "application/vnd.ms-htmlhelp", IsBinary = true)]
        CHM,

        [ContentTypeMetadata(Value = "application/vnd.kde.kchart", IsBinary = true)]
        CHRT,

        [ContentTypeMetadata(Value = "chemical/x-cif", IsBinary = true)]
        CIF,

        [ContentTypeMetadata(Value = "application/vnd.anser-web-certificate-issue-initiation", IsBinary = true)]
        CII,

        [ContentTypeMetadata(Value = "application/vnd.ms-artgalry", IsBinary = true)]
        CIL,

        [ContentTypeMetadata(Value = "application/vnd.claymore", IsBinary = true)]
        CLA,

        [ContentTypeMetadata(Value = "application/java-vm", IsBinary = true)]
        CLASS,

        [ContentTypeMetadata(Value = "application/vnd.crick.clicker.keyboard", IsBinary = true)]
        CLKK,

        [ContentTypeMetadata(Value = "application/vnd.crick.clicker.palette", IsBinary = true)]
        CLKP,

        [ContentTypeMetadata(Value = "application/vnd.crick.clicker.template", IsBinary = true)]
        CLKT,

        [ContentTypeMetadata(Value = "application/vnd.crick.clicker.wordbank", IsBinary = true)]
        CLKW,

        [ContentTypeMetadata(Value = "application/vnd.crick.clicker", IsBinary = true)]
        CLKX,

        [ContentTypeMetadata(Value = "application/x-msclip", IsBinary = true)]
        CLP,

        [ContentTypeMetadata(Value = "application/vnd.cosmocaller", IsBinary = true)]
        CMC,

        [ContentTypeMetadata(Value = "chemical/x-cmdf", IsBinary = true)]
        CMDF,

        [ContentTypeMetadata(Value = "chemical/x-cml", IsBinary = true)]
        CML,

        [ContentTypeMetadata(Value = "application/vnd.yellowriver-custom-menu", IsBinary = true)]
        CMP,

        [ContentTypeMetadata(Value = "image/x-cmx", IsBinary = true)]
        CMX,

        [ContentTypeMetadata(Value = "application/vnd.rim.cod", IsBinary = true)]
        COD,

        [ContentTypeMetadata(Value = "application/x-msdownload", IsBinary = true)]
        COM,

        [ContentTypeMetadata(Value = "text/plain", IsText = true)]
        CONF,

        [ContentTypeMetadata(Value = "application/x-cpio", IsBinary = true)]
        CPIO,

        [ContentTypeMetadata(Value = "text/x-c", IsText = true)]
        CPP,

        [ContentTypeMetadata(Value = "application/mac-compactpro", IsBinary = true)]
        CPT,

        [ContentTypeMetadata(Value = "application/x-mscardfile", IsBinary = true)]
        CRD,

        [ContentTypeMetadata(Value = "application/pkix-crl", IsBinary = true)]
        CRL,

        [ContentTypeMetadata(Value = "application/x-x509-ca-cert", IsBinary = true)]
        CRT,

        [ContentTypeMetadata(Value = "application/vnd.rig.cryptonote", IsBinary = true)]
        CRYPTONOTE,

        [ContentTypeMetadata(Value = "application/x-csh", IsBinary = true)]
        CSH,

        [ContentTypeMetadata(Value = "chemical/x-csml", IsBinary = true)]
        CSML,

        [ContentTypeMetadata(Value = "application/vnd.commonspace", IsBinary = true)]
        CSP,

        [ContentTypeMetadata(Value = "text/css", IsText = true)]
        CSS,

        [ContentTypeMetadata(Value = "application/x-director", IsBinary = true)]
        CST,

        [ContentTypeMetadata(Value = "text/csv", IsText = true)]
        CSV,

        [ContentTypeMetadata(Value = "application/cu-seeme", IsBinary = true)]
        CU,

        [ContentTypeMetadata(Value = "text/vnd.curl", IsText = true)]
        CURL,

        [ContentTypeMetadata(Value = "application/prs.cww", IsBinary = true)]
        CWW,

        [ContentTypeMetadata(Value = "application/x-director", IsBinary = true)]
        CXT,

        [ContentTypeMetadata(Value = "text/x-c", IsText = true)]
        CXX,

        [ContentTypeMetadata(Value = "model/vnd.collada+xml", IsText = true)]
        DAE,

        [ContentTypeMetadata(Value = "application/vnd.mobius.daf", IsBinary = true)]
        DAF,

        [ContentTypeMetadata(Value = "application/vnd.dart", IsBinary = true)]
        DART,

        [ContentTypeMetadata(Value = "application/vnd.fdsn.seed", IsBinary = true)]
        DATALESS,

        [ContentTypeMetadata(Value = "application/davmount+xml", IsText = true)]
        DAVMOUNT,

        [ContentTypeMetadata(Value = "application/docbook+xml", IsText = true)]
        DBK,

        [ContentTypeMetadata(Value = "application/x-director", IsBinary = true)]
        DCR,

        [ContentTypeMetadata(Value = "text/vnd.curl.dcurl", IsText = true)]
        DCURL,

        [ContentTypeMetadata(Value = "application/vnd.oma.dd2+xml", IsText = true)]
        DD2,

        [ContentTypeMetadata(Value = "application/vnd.fujixerox.ddd", IsBinary = true)]
        DDD,

        [ContentTypeMetadata(Value = "application/x-debian-package", IsBinary = true)]
        DEB,

        [ContentTypeMetadata(Value = "text/plain", IsText = true)]
        DEF,

        [ContentTypeMetadata(Value = "application/octet-stream", IsBinary = true)]
        DEPLOY,

        [ContentTypeMetadata(Value = "application/x-x509-ca-cert", IsBinary = true)]
        DER,

        [ContentTypeMetadata(Value = "application/vnd.dreamfactory", IsBinary = true)]
        DFAC,

        [ContentTypeMetadata(Value = "application/x-dgc-compressed", IsBinary = true)]
        DGC,

        [ContentTypeMetadata(Value = "text/x-c", IsText = true)]
        DIC,

        [ContentTypeMetadata(Value = "video/x-dv", IsBinary = true)]
        DIF,

        [ContentTypeMetadata(Value = "application/x-director", IsBinary = true)]
        DIR,

        [ContentTypeMetadata(Value = "application/vnd.mobius.dis", IsBinary = true)]
        DIS,

        [ContentTypeMetadata(Value = "application/octet-stream", IsBinary = true)]
        DIST,

        [ContentTypeMetadata(Value = "application/octet-stream", IsBinary = true)]
        DISTZ,

        [ContentTypeMetadata(Value = "image/vnd.djvu", IsBinary = true)]
        DJV,

        [ContentTypeMetadata(Value = "image/vnd.djvu", IsBinary = true)]
        DJVU,

        [ContentTypeMetadata(Value = "application/x-msdownload", IsBinary = true)]
        DLL,

        [ContentTypeMetadata(Value = "application/x-apple-diskimage", IsBinary = true)]
        DMG,

        [ContentTypeMetadata(Value = "application/vnd.tcpdump.pcap", IsBinary = true)]
        DMP,

        [ContentTypeMetadata(Value = "application/octet-stream", IsBinary = true)]
        DMS,

        [ContentTypeMetadata(Value = "application/vnd.dna", IsBinary = true)]
        DNA,

        [ContentTypeMetadata(Value = "application/msword", IsBinary = true)]
        DOC,

        [ContentTypeMetadata(Value = "application/vnd.ms-word.document.macroenabled.12", IsBinary = true)]
        DOCM,

        [ContentTypeMetadata(Value = "application/vnd.openxmlformats-officedocument.wordprocessingml.document", IsBinary = true)]
        DOCX,

        [ContentTypeMetadata(Value = "application/msword", IsBinary = true)]
        DOT,

        [ContentTypeMetadata(Value = "application/vnd.ms-word.template.macroenabled.12", IsBinary = true)]
        DOTM,

        [ContentTypeMetadata(Value = "application/vnd.openxmlformats-officedocument.wordprocessingml.template", IsBinary = true)]
        DOTX,

        [ContentTypeMetadata(Value = "application/vnd.osgi.dp", IsBinary = true)]
        DP,

        [ContentTypeMetadata(Value = "application/vnd.dpgraph", IsBinary = true)]
        DPG,

        [ContentTypeMetadata(Value = "audio/vnd.dra", IsBinary = true)]
        DRA,

        [ContentTypeMetadata(Value = "text/prs.lines.tag", IsText = true)]
        DSC,

        [ContentTypeMetadata(Value = "application/dssc+der", IsBinary = true)]
        DSSC,

        [ContentTypeMetadata(Value = "application/x-dtbook+xml", IsText = true)]
        DTB,

        [ContentTypeMetadata(Value = "application/xml-dtd", IsBinary = true)]
        DTD,

        [ContentTypeMetadata(Value = "audio/vnd.dts", IsBinary = true)]
        DTS,

        [ContentTypeMetadata(Value = "audio/vnd.dts.hd", IsBinary = true)]
        DTSHD,

        [ContentTypeMetadata(Value = "application/octet-stream", IsBinary = true)]
        DUMP,

        [ContentTypeMetadata(Value = "video/x-dv", IsBinary = true)]
        DV,

        [ContentTypeMetadata(Value = "video/vnd.dvb.file", IsBinary = true)]
        DVB,

        [ContentTypeMetadata(Value = "application/x-dvi", IsBinary = true)]
        DVI,

        [ContentTypeMetadata(Value = "model/vnd.dwf", IsBinary = true)]
        DWF,

        [ContentTypeMetadata(Value = "image/vnd.dwg", IsBinary = true)]
        DWG,

        [ContentTypeMetadata(Value = "image/vnd.dxf", IsBinary = true)]
        DXF,

        [ContentTypeMetadata(Value = "application/vnd.spotfire.dxp", IsBinary = true)]
        DXP,

        [ContentTypeMetadata(Value = "application/x-director", IsBinary = true)]
        DXR,

        [ContentTypeMetadata(Value = "audio/vnd.nuera.ecelp4800", IsBinary = true)]
        ECELP4800,

        [ContentTypeMetadata(Value = "audio/vnd.nuera.ecelp7470", IsBinary = true)]
        ECELP7470,

        [ContentTypeMetadata(Value = "audio/vnd.nuera.ecelp9600", IsBinary = true)]
        ECELP9600,

        [ContentTypeMetadata(Value = "application/ecmascript", IsBinary = true)]
        ECMA,

        [ContentTypeMetadata(Value = "application/vnd.novadigm.edm", IsBinary = true)]
        EDM,

        [ContentTypeMetadata(Value = "application/vnd.novadigm.edx", IsBinary = true)]
        EDX,

        [ContentTypeMetadata(Value = "application/vnd.picsel", IsBinary = true)]
        EFIF,

        [ContentTypeMetadata(Value = "application/vnd.pg.osasli", IsBinary = true)]
        EI6,

        [ContentTypeMetadata(Value = "application/octet-stream", IsBinary = true)]
        ELC,

        [ContentTypeMetadata(Value = "application/x-msmetafile", IsBinary = true)]
        EMF,

        [ContentTypeMetadata(Value = "message/rfc822", IsBinary = true)]
        EML,

        [ContentTypeMetadata(Value = "application/emma+xml", IsText = true)]
        EMMA,

        [ContentTypeMetadata(Value = "application/x-msmetafile", IsBinary = true)]
        EMZ,

        [ContentTypeMetadata(Value = "audio/vnd.digital-winds", IsBinary = true)]
        EOL,

        [ContentTypeMetadata(Value = "application/vnd.ms-fontobject", IsBinary = true)]
        EOT,

        [ContentTypeMetadata(Value = "application/postscript", IsBinary = true)]
        EPS,

        [ContentTypeMetadata(Value = "application/epub+zip", IsBinary = true)]
        EPUB,

        [ContentTypeMetadata(Value = "application/vnd.eszigno3+xml", IsText = true)]
        ES3,

        [ContentTypeMetadata(Value = "application/vnd.osgi.subsystem", IsBinary = true)]
        ESA,

        [ContentTypeMetadata(Value = "application/vnd.epson.esf", IsBinary = true)]
        ESF,

        [ContentTypeMetadata(Value = "application/vnd.eszigno3+xml", IsText = true)]
        ET3,

        [ContentTypeMetadata(Value = "text/x-setext", IsText = true)]
        ETX,

        [ContentTypeMetadata(Value = "application/x-eva", IsBinary = true)]
        EVA,

        [ContentTypeMetadata(Value = "application/x-envoy", IsBinary = true)]
        EVY,

        [ContentTypeMetadata(Value = "application/x-msdownload", IsBinary = true)]
        EXE,

        [ContentTypeMetadata(Value = "application/exi", IsBinary = true)]
        EXI,

        [ContentTypeMetadata(Value = "application/vnd.novadigm.ext", IsBinary = true)]
        EXT,

        [ContentTypeMetadata(Value = "MIME type (lowercased)", IsBinary = true)]
        EXTENSIONS,

        [ContentTypeMetadata(Value = "application/andrew-inset", IsBinary = true)]
        EZ,

        [ContentTypeMetadata(Value = "application/vnd.ezpix-album", IsBinary = true)]
        EZ2,

        [ContentTypeMetadata(Value = "application/vnd.ezpix-package", IsBinary = true)]
        EZ3,

        [ContentTypeMetadata(Value = "text/x-fortran", IsText = true)]
        F,

        [ContentTypeMetadata(Value = "video/x-f4v", IsBinary = true)]
        F4V,

        [ContentTypeMetadata(Value = "text/x-fortran", IsText = true)]
        F77,

        [ContentTypeMetadata(Value = "text/x-fortran", IsText = true)]
        F90,

        [ContentTypeMetadata(Value = "image/vnd.fastbidsheet", IsBinary = true)]
        FBS,

        [ContentTypeMetadata(Value = "application/vnd.adobe.formscentral.fcdt", IsBinary = true)]
        FCDT,

        [ContentTypeMetadata(Value = "application/vnd.isac.fcs", IsBinary = true)]
        FCS,

        [ContentTypeMetadata(Value = "application/vnd.fdf", IsBinary = true)]
        FDF,

        [ContentTypeMetadata(Value = "application/vnd.denovo.fcselayout-link", IsBinary = true)]
        FE_LAUNCH,

        [ContentTypeMetadata(Value = "application/vnd.fujitsu.oasysgp", IsBinary = true)]
        FG5,

        [ContentTypeMetadata(Value = "application/x-director", IsBinary = true)]
        FGD,

        [ContentTypeMetadata(Value = "image/x-freehand", IsBinary = true)]
        FH,

        [ContentTypeMetadata(Value = "image/x-freehand", IsBinary = true)]
        FH4,

        [ContentTypeMetadata(Value = "image/x-freehand", IsBinary = true)]
        FH5,

        [ContentTypeMetadata(Value = "image/x-freehand", IsBinary = true)]
        FH7,

        [ContentTypeMetadata(Value = "image/x-freehand", IsBinary = true)]
        FHC,

        [ContentTypeMetadata(Value = "application/x-xfig", IsBinary = true)]
        FIG,

        [ContentTypeMetadata(Value = "audio/x-flac", IsBinary = true)]
        FLAC,

        [ContentTypeMetadata(Value = "video/x-fli", IsBinary = true)]
        FLI,

        [ContentTypeMetadata(Value = "application/vnd.micrografx.flo", IsBinary = true)]
        FLO,

        [ContentTypeMetadata(Value = "video/x-flv", IsBinary = true)]
        FLV,

        [ContentTypeMetadata(Value = "application/vnd.kde.kivio", IsBinary = true)]
        FLW,

        [ContentTypeMetadata(Value = "text/vnd.fmi.flexstor", IsText = true)]
        FLX,

        [ContentTypeMetadata(Value = "text/vnd.fly", IsText = true)]
        FLY,

        [ContentTypeMetadata(Value = "application/vnd.framemaker", IsBinary = true)]
        FM,

        [ContentTypeMetadata(Value = "application/vnd.frogans.fnc", IsBinary = true)]
        FNC,

        [ContentTypeMetadata(Value = "text/x-fortran", IsText = true)]
        FOR,

        [ContentTypeMetadata(Value = "image/vnd.fpx", IsBinary = true)]
        FPX,

        [ContentTypeMetadata(Value = "application/vnd.framemaker", IsBinary = true)]
        FRAME,

        [ContentTypeMetadata(Value = "application/vnd.fsc.weblaunch", IsBinary = true)]
        FSC,

        [ContentTypeMetadata(Value = "image/vnd.fst", IsBinary = true)]
        FST,

        [ContentTypeMetadata(Value = "application/vnd.fluxtime.clip", IsBinary = true)]
        FTC,

        [ContentTypeMetadata(Value = "application/vnd.anser-web-funds-transfer-initiation", IsBinary = true)]
        FTI,

        [ContentTypeMetadata(Value = "video/vnd.fvt", IsBinary = true)]
        FVT,

        [ContentTypeMetadata(Value = "application/vnd.adobe.fxp", IsBinary = true)]
        FXP,

        [ContentTypeMetadata(Value = "application/vnd.adobe.fxp", IsBinary = true)]
        FXPL,

        [ContentTypeMetadata(Value = "application/vnd.fuzzysheet", IsBinary = true)]
        FZS,

        [ContentTypeMetadata(Value = "application/vnd.geoplan", IsBinary = true)]
        G2W,

        [ContentTypeMetadata(Value = "image/g3fax", IsBinary = true)]
        G3,

        [ContentTypeMetadata(Value = "application/vnd.geospace", IsBinary = true)]
        G3W,

        [ContentTypeMetadata(Value = "application/vnd.groove-account", IsBinary = true)]
        GAC,

        [ContentTypeMetadata(Value = "application/x-tads", IsBinary = true)]
        GAM,

        [ContentTypeMetadata(Value = "application/rpki-ghostbusters", IsBinary = true)]
        GBR,

        [ContentTypeMetadata(Value = "application/x-gca-compressed", IsBinary = true)]
        GCA,

        [ContentTypeMetadata(Value = "model/vnd.gdl", IsBinary = true)]
        GDL,

        [ContentTypeMetadata(Value = "application/vnd.dynageo", IsBinary = true)]
        GEO,

        [ContentTypeMetadata(Value = "application/vnd.geometry-explorer", IsBinary = true)]
        GEX,

        [ContentTypeMetadata(Value = "application/vnd.geogebra.file", IsBinary = true)]
        GGB,

        [ContentTypeMetadata(Value = "application/vnd.geogebra.tool", IsBinary = true)]
        GGT,

        [ContentTypeMetadata(Value = "application/vnd.groove-help", IsBinary = true)]
        GHF,

        [ContentTypeMetadata(Value = "image/gif", IsBinary = true)]
        GIF,

        [ContentTypeMetadata(Value = "application/vnd.groove-identity-message", IsBinary = true)]
        GIM,

        [ContentTypeMetadata(Value = "application/gml+xml", IsText = true)]
        GML,

        [ContentTypeMetadata(Value = "application/vnd.gmx", IsBinary = true)]
        GMX,

        [ContentTypeMetadata(Value = "application/x-gnumeric", IsBinary = true)]
        GNUMERIC,

        [ContentTypeMetadata(Value = "application/vnd.flographit", IsBinary = true)]
        GPH,

        [ContentTypeMetadata(Value = "application/gpx+xml", IsText = true)]
        GPX,

        [ContentTypeMetadata(Value = "application/vnd.grafeq", IsBinary = true)]
        GQF,

        [ContentTypeMetadata(Value = "application/vnd.grafeq", IsBinary = true)]
        GQS,

        [ContentTypeMetadata(Value = "application/srgs", IsBinary = true)]
        GRAM,

        [ContentTypeMetadata(Value = "application/x-gramps-xml", IsText = true)]
        GRAMPS,

        [ContentTypeMetadata(Value = "application/vnd.geometry-explorer", IsBinary = true)]
        GRE,

        [ContentTypeMetadata(Value = "application/vnd.groove-injector", IsBinary = true)]
        GRV,

        [ContentTypeMetadata(Value = "application/srgs+xml", IsText = true)]
        GRXML,

        [ContentTypeMetadata(Value = "application/x-font-ghostscript", IsBinary = true)]
        GSF,

        [ContentTypeMetadata(Value = "application/x-gtar", IsBinary = true)]
        GTAR,

        [ContentTypeMetadata(Value = "application/vnd.groove-tool-message", IsBinary = true)]
        GTM,

        [ContentTypeMetadata(Value = "model/vnd.gtw", IsBinary = true)]
        GTW,

        [ContentTypeMetadata(Value = "text/vnd.graphviz", IsText = true)]
        GV,

        [ContentTypeMetadata(Value = "application/gxf", IsBinary = true)]
        GXF,

        [ContentTypeMetadata(Value = "application/vnd.geonext", IsBinary = true)]
        GXT,

        [ContentTypeMetadata(Value = "text/x-c", IsText = true)]
        H,

        [ContentTypeMetadata(Value = "video/h261", IsBinary = true)]
        H261,

        [ContentTypeMetadata(Value = "video/h263", IsBinary = true)]
        H263,

        [ContentTypeMetadata(Value = "video/h264", IsBinary = true)]
        H264,

        [ContentTypeMetadata(Value = "application/vnd.hal+xml", IsText = true)]
        HAL,

        [ContentTypeMetadata(Value = "application/vnd.hbci", IsBinary = true)]
        HBCI,

        [ContentTypeMetadata(Value = "application/x-hdf", IsBinary = true)]
        HDF,

        [ContentTypeMetadata(Value = "text/x-c", IsText = true)]
        HH,

        [ContentTypeMetadata(Value = "application/winhlp", IsBinary = true)]
        HLP,

        [ContentTypeMetadata(Value = "application/vnd.hp-hpgl", IsBinary = true)]
        HPGL,

        [ContentTypeMetadata(Value = "application/vnd.hp-hpid", IsBinary = true)]
        HPID,

        [ContentTypeMetadata(Value = "application/vnd.hp-hps", IsBinary = true)]
        HPS,

        [ContentTypeMetadata(Value = "application/mac-binhex40", IsBinary = true)]
        HQX,

        [ContentTypeMetadata(Value = "application/vnd.kenameaapp", IsBinary = true)]
        HTKE,

        [ContentTypeMetadata(Value = "text/html", IsText = true)]
        HTM,

        [ContentTypeMetadata(Value = "text/html", IsText = true)]
        HTML,

        [ContentTypeMetadata(Value = "application/vnd.yamaha.hv-dic", IsBinary = true)]
        HVD,

        [ContentTypeMetadata(Value = "application/vnd.yamaha.hv-voice", IsBinary = true)]
        HVP,

        [ContentTypeMetadata(Value = "application/vnd.yamaha.hv-script", IsBinary = true)]
        HVS,

        [ContentTypeMetadata(Value = "application/vnd.intergeo", IsBinary = true)]
        I2G,

        [ContentTypeMetadata(Value = "x-conference/x-cooltalk", IsBinary = true)]
        IC,

        [ContentTypeMetadata(Value = "application/vnd.iccprofile", IsBinary = true)]
        ICC,

        [ContentTypeMetadata(Value = "x-conference/x-cooltalk", IsBinary = true)]
        ICE,

        [ContentTypeMetadata(Value = "application/vnd.iccprofile", IsBinary = true)]
        ICM,

        [ContentTypeMetadata(Value = "image/x-icon", IsBinary = true)]
        ICO,

        [ContentTypeMetadata(Value = "text/calendar", IsText = true)]
        ICS,

        [ContentTypeMetadata(Value = "image/ief", IsBinary = true)]
        IEF,

        [ContentTypeMetadata(Value = "text/calendar", IsText = true)]
        IFB,

        [ContentTypeMetadata(Value = "application/vnd.shana.informed.formdata", IsBinary = true)]
        IFM,

        [ContentTypeMetadata(Value = "model/iges", IsBinary = true)]
        IGES,

        [ContentTypeMetadata(Value = "application/vnd.igloader", IsBinary = true)]
        IGL,

        [ContentTypeMetadata(Value = "application/vnd.insors.igm", IsBinary = true)]
        IGM,

        [ContentTypeMetadata(Value = "model/iges", IsBinary = true)]
        IGS,

        [ContentTypeMetadata(Value = "application/vnd.micrografx.igx", IsBinary = true)]
        IGX,

        [ContentTypeMetadata(Value = "application/vnd.shana.informed.interchange", IsBinary = true)]
        IIF,

        [ContentTypeMetadata(Value = "application/vnd.accpac.simply.imp", IsBinary = true)]
        IMP,

        [ContentTypeMetadata(Value = "application/vnd.ms-ims", IsBinary = true)]
        IMS,

        [ContentTypeMetadata(Value = "text/plain", IsText = true)]
        IN,

        [ContentTypeMetadata(Value = "application/inkml+xml", IsText = true)]
        INK,

        [ContentTypeMetadata(Value = "application/inkml+xml", IsText = true)]
        INKML,

        [ContentTypeMetadata(Value = "application/x-install-instructions", IsBinary = true)]
        INSTALL,

        [ContentTypeMetadata(Value = "application/vnd.astraea-software.iota", IsBinary = true)]
        IOTA,

        [ContentTypeMetadata(Value = "application/ipfix", IsBinary = true)]
        IPFIX,

        [ContentTypeMetadata(Value = "application/vnd.shana.informed.package", IsBinary = true)]
        IPK,

        [ContentTypeMetadata(Value = "application/vnd.ibm.rights-management", IsBinary = true)]
        IRM,

        [ContentTypeMetadata(Value = "application/vnd.irepository.package+xml", IsText = true)]
        IRP,

        [ContentTypeMetadata(Value = "application/x-iso9660-image", IsBinary = true)]
        ISO,

        [ContentTypeMetadata(Value = "application/vnd.shana.informed.formtemplate", IsBinary = true)]
        ITP,

        [ContentTypeMetadata(Value = "application/vnd.immervision-ivp", IsBinary = true)]
        IVP,

        [ContentTypeMetadata(Value = "application/vnd.immervision-ivu", IsBinary = true)]
        IVU,

        [ContentTypeMetadata(Value = "text/vnd.sun.j2me.app-descriptor", IsText = true)]
        JAD,

        [ContentTypeMetadata(Value = "application/vnd.jam", IsBinary = true)]
        JAM,

        [ContentTypeMetadata(Value = "application/java-archive", IsBinary = true)]
        JAR,

        [ContentTypeMetadata(Value = "text/x-java-source", IsText = true)]
        JAVA,

        [ContentTypeMetadata(Value = "application/vnd.jisp", IsBinary = true)]
        JISP,

        [ContentTypeMetadata(Value = "application/vnd.hp-jlyt", IsBinary = true)]
        JLT,

        [ContentTypeMetadata(Value = "application/x-java-jnlp-file", IsBinary = true)]
        JNLP,

        [ContentTypeMetadata(Value = "application/vnd.joost.joda-archive", IsBinary = true)]
        JODA,

        [ContentTypeMetadata(Value = "image/jp2", IsBinary = true)]
        JP2,

        [ContentTypeMetadata(Value = "image/jpeg", IsBinary = true)]
        JPE,

        [ContentTypeMetadata(Value = "image/jpeg", IsBinary = true)]
        JPEG,

        [ContentTypeMetadata(Value = "image/jpeg", IsBinary = true)]
        JPG,

        [ContentTypeMetadata(Value = "video/jpm", IsBinary = true)]
        JPGM,

        [ContentTypeMetadata(Value = "video/jpeg", IsBinary = true)]
        JPGV,

        [ContentTypeMetadata(Value = "video/jpm", IsBinary = true)]
        JPM,

        [ContentTypeMetadata(Value = "application/javascript", IsText = true)]
        JS,

        [ContentTypeMetadata(Value = "application/json", IsText = true)]
        JSON,

        [ContentTypeMetadata(Value = "application/jsonml+json", IsText = true)]
        JSONML,

        [ContentTypeMetadata(Value = "audio/midi", IsBinary = true)]
        KAR,

        [ContentTypeMetadata(Value = "application/vnd.kde.karbon", IsBinary = true)]
        KARBON,

        [ContentTypeMetadata(Value = "application/vnd.kde.kformula", IsBinary = true)]
        KFO,

        [ContentTypeMetadata(Value = "application/vnd.kidspiration", IsBinary = true)]
        KIA,

        [ContentTypeMetadata(Value = "application/vnd.google-earth.kml+xml", IsText = true)]
        KML,

        [ContentTypeMetadata(Value = "application/vnd.google-earth.kmz", IsBinary = true)]
        KMZ,

        [ContentTypeMetadata(Value = "application/vnd.kinar", IsBinary = true)]
        KNE,

        [ContentTypeMetadata(Value = "application/vnd.kinar", IsBinary = true)]
        KNP,

        [ContentTypeMetadata(Value = "application/vnd.kde.kontour", IsBinary = true)]
        KON,

        [ContentTypeMetadata(Value = "application/vnd.kde.kpresenter", IsBinary = true)]
        KPR,

        [ContentTypeMetadata(Value = "application/vnd.kde.kpresenter", IsBinary = true)]
        KPT,

        [ContentTypeMetadata(Value = "application/vnd.ds-keypoint", IsBinary = true)]
        KPXX,

        [ContentTypeMetadata(Value = "application/vnd.kde.kspread", IsBinary = true)]
        KSP,

        [ContentTypeMetadata(Value = "application/vnd.kahootz", IsBinary = true)]
        KTR,

        [ContentTypeMetadata(Value = "image/ktx", IsBinary = true)]
        KTX,

        [ContentTypeMetadata(Value = "application/vnd.kahootz", IsBinary = true)]
        KTZ,

        [ContentTypeMetadata(Value = "application/vnd.kde.kword", IsBinary = true)]
        KWD,

        [ContentTypeMetadata(Value = "application/vnd.kde.kword", IsBinary = true)]
        KWT,

        [ContentTypeMetadata(Value = "application/vnd.las.las+xml", IsText = true)]
        LASXML,

        [ContentTypeMetadata(Value = "application/x-latex", IsBinary = true)]
        LATEX,

        [ContentTypeMetadata(Value = "application/vnd.llamagraphics.life-balance.desktop", IsBinary = true)]
        LBD,

        [ContentTypeMetadata(Value = "application/vnd.llamagraphics.life-balance.exchange+xml", IsText = true)]
        LBE,

        [ContentTypeMetadata(Value = "application/vnd.hhe.lesson-player", IsBinary = true)]
        LES,

        [ContentTypeMetadata(Value = "application/x-lzh-compressed", IsBinary = true)]
        LHA,

        [ContentTypeMetadata(Value = "application/vnd.route66.link66+xml", IsText = true)]
        LINK66,

        [ContentTypeMetadata(Value = "text/plain", IsText = true)]
        LIST,

        [ContentTypeMetadata(Value = "application/vnd.ibm.modcap", IsBinary = true)]
        LIST3820,

        [ContentTypeMetadata(Value = "application/vnd.ibm.modcap", IsBinary = true)]
        LISTAFP,

        [ContentTypeMetadata(Value = "application/x-ms-shortcut", IsBinary = true)]
        LNK,

        [ContentTypeMetadata(Value = "text/plain", IsText = true)]
        LOG,

        [ContentTypeMetadata(Value = "application/lost+xml", IsText = true)]
        LOSTXML,

        [ContentTypeMetadata(Value = "application/octet-stream", IsBinary = true)]
        LRF,

        [ContentTypeMetadata(Value = "application/vnd.ms-lrm", IsBinary = true)]
        LRM,

        [ContentTypeMetadata(Value = "application/vnd.frogans.ltf", IsBinary = true)]
        LTF,

        [ContentTypeMetadata(Value = "audio/vnd.lucent.voice", IsBinary = true)]
        LVP,

        [ContentTypeMetadata(Value = "application/vnd.lotus-wordpro", IsBinary = true)]
        LWP,

        [ContentTypeMetadata(Value = "application/x-lzh-compressed", IsBinary = true)]
        LZH,

        [ContentTypeMetadata(Value = "application/x-msmediaview", IsBinary = true)]
        M13,

        [ContentTypeMetadata(Value = "application/x-msmediaview", IsBinary = true)]
        M14,

        [ContentTypeMetadata(Value = "video/mpeg", IsBinary = true)]
        M1V,

        [ContentTypeMetadata(Value = "application/mp21", IsBinary = true)]
        M21,

        [ContentTypeMetadata(Value = "audio/mpeg", IsBinary = true)]
        M2A,

        [ContentTypeMetadata(Value = "video/mpeg", IsBinary = true)]
        M2V,

        [ContentTypeMetadata(Value = "audio/mpeg", IsBinary = true)]
        M3A,

        [ContentTypeMetadata(Value = "audio/x-mpegurl", IsBinary = true)]
        M3U,

        [ContentTypeMetadata(Value = "application/vnd.apple.mpegurl", IsBinary = true)]
        M3U8,

        [ContentTypeMetadata(Value = "audio/mp4a-latm", IsBinary = true)]
        M4A,

        [ContentTypeMetadata(Value = "audio/mp4a-latm", IsBinary = true)]
        M4B,

        [ContentTypeMetadata(Value = "audio/mp4a-latm", IsBinary = true)]
        M4P,

        [ContentTypeMetadata(Value = "video/vnd.mpegurl", IsBinary = true)]
        M4U,

        [ContentTypeMetadata(Value = "video/x-m4v", IsBinary = true)]
        M4V,

        [ContentTypeMetadata(Value = "application/mathematica", IsBinary = true)]
        MA,

        [ContentTypeMetadata(Value = "image/x-macpaint", IsBinary = true)]
        MAC,

        [ContentTypeMetadata(Value = "application/mads+xml", IsText = true)]
        MADS,

        [ContentTypeMetadata(Value = "application/vnd.ecowin.chart", IsBinary = true)]
        MAG,

        [ContentTypeMetadata(Value = "application/vnd.framemaker", IsBinary = true)]
        MAKER,

        [ContentTypeMetadata(Value = "application/x-troff-man", IsBinary = true)]
        MAN,

        [ContentTypeMetadata(Value = "application/octet-stream", IsBinary = true)]
        MAR,

        [ContentTypeMetadata(Value = "application/mathml+xml", IsText = true)]
        MATHML,

        [ContentTypeMetadata(Value = "application/mathematica", IsBinary = true)]
        MB,

        [ContentTypeMetadata(Value = "application/vnd.mobius.mbk", IsBinary = true)]
        MBK,

        [ContentTypeMetadata(Value = "application/mbox", IsBinary = true)]
        MBOX,

        [ContentTypeMetadata(Value = "application/vnd.medcalcdata", IsBinary = true)]
        MC1,

        [ContentTypeMetadata(Value = "application/vnd.mcd", IsBinary = true)]
        MCD,

        [ContentTypeMetadata(Value = "text/vnd.curl.mcurl", IsText = true)]
        MCURL,

        [ContentTypeMetadata(Value = "application/x-msaccess", IsBinary = true)]
        MDB,

        [ContentTypeMetadata(Value = "image/vnd.ms-modi", IsBinary = true)]
        MDI,

        [ContentTypeMetadata(Value = "application/x-troff-me", IsBinary = true)]
        ME,

        [ContentTypeMetadata(Value = "model/mesh", IsBinary = true)]
        MESH,

        [ContentTypeMetadata(Value = "application/metalink4+xml", IsText = true)]
        META4,

        [ContentTypeMetadata(Value = "application/metalink+xml", IsText = true)]
        METALINK,

        [ContentTypeMetadata(Value = "application/mets+xml", IsText = true)]
        METS,

        [ContentTypeMetadata(Value = "application/vnd.mfmp", IsBinary = true)]
        MFM,

        [ContentTypeMetadata(Value = "application/rpki-manifest", IsBinary = true)]
        MFT,

        [ContentTypeMetadata(Value = "application/vnd.osgeo.mapguide.package", IsBinary = true)]
        MGP,

        [ContentTypeMetadata(Value = "application/vnd.proteus.magazine", IsBinary = true)]
        MGZ,

        [ContentTypeMetadata(Value = "audio/midi", IsBinary = true)]
        MID,

        [ContentTypeMetadata(Value = "audio/midi", IsBinary = true)]
        MIDI,

        [ContentTypeMetadata(Value = "application/x-mie", IsBinary = true)]
        MIE,

        [ContentTypeMetadata(Value = "application/vnd.mif", IsBinary = true)]
        MIF,

        [ContentTypeMetadata(Value = "message/rfc822", IsBinary = true)]
        MIME,

        [ContentTypeMetadata(Value = "video/mj2", IsBinary = true)]
        MJ2,

        [ContentTypeMetadata(Value = "video/mj2", IsBinary = true)]
        MJP2,

        [ContentTypeMetadata(Value = "video/x-matroska", IsBinary = true)]
        MK3D,

        [ContentTypeMetadata(Value = "audio/x-matroska", IsBinary = true)]
        MKA,

        [ContentTypeMetadata(Value = "video/x-matroska", IsBinary = true)]
        MKS,

        [ContentTypeMetadata(Value = "video/x-matroska", IsBinary = true)]
        MKV,

        [ContentTypeMetadata(Value = "application/vnd.dolby.mlp", IsBinary = true)]
        MLP,

        [ContentTypeMetadata(Value = "application/vnd.chipnuts.karaoke-mmd", IsBinary = true)]
        MMD,

        [ContentTypeMetadata(Value = "application/vnd.smaf", IsBinary = true)]
        MMF,

        [ContentTypeMetadata(Value = "image/vnd.fujixerox.edmics-mmr", IsBinary = true)]
        MMR,

        [ContentTypeMetadata(Value = "video/x-mng", IsBinary = true)]
        MNG,

        [ContentTypeMetadata(Value = "application/x-msmoney", IsBinary = true)]
        MNY,

        [ContentTypeMetadata(Value = "application/x-mobipocket-ebook", IsBinary = true)]
        MOBI,

        [ContentTypeMetadata(Value = "application/mods+xml", IsText = true)]
        MODS,

        [ContentTypeMetadata(Value = "video/quicktime", IsBinary = true)]
        MOV,

        [ContentTypeMetadata(Value = "video/x-sgi-movie", IsBinary = true)]
        MOVIE,

        [ContentTypeMetadata(Value = "audio/mpeg", IsBinary = true)]
        MP2,

        [ContentTypeMetadata(Value = "application/mp21", IsBinary = true)]
        MP21,

        [ContentTypeMetadata(Value = "audio/mpeg", IsBinary = true)]
        MP2A,

        [ContentTypeMetadata(Value = "audio/mpeg", IsBinary = true)]
        MP3,

        [ContentTypeMetadata(Value = "video/mp4", IsBinary = true)]
        MP4,

        [ContentTypeMetadata(Value = "audio/mp4", IsBinary = true)]
        MP4A,

        [ContentTypeMetadata(Value = "application/mp4", IsBinary = true)]
        MP4S,

        [ContentTypeMetadata(Value = "video/mp4", IsBinary = true)]
        MP4V,

        [ContentTypeMetadata(Value = "application/vnd.mophun.certificate", IsBinary = true)]
        MPC,

        [ContentTypeMetadata(Value = "video/mpeg", IsBinary = true)]
        MPE,

        [ContentTypeMetadata(Value = "video/mpeg", IsBinary = true)]
        MPEG,

        [ContentTypeMetadata(Value = "video/mpeg", IsBinary = true)]
        MPG,

        [ContentTypeMetadata(Value = "video/mp4", IsBinary = true)]
        MPG4,

        [ContentTypeMetadata(Value = "audio/mpeg", IsBinary = true)]
        MPGA,

        [ContentTypeMetadata(Value = "application/vnd.apple.installer+xml", IsText = true)]
        MPKG,

        [ContentTypeMetadata(Value = "application/vnd.blueice.multipass", IsBinary = true)]
        MPM,

        [ContentTypeMetadata(Value = "application/vnd.mophun.application", IsBinary = true)]
        MPN,

        [ContentTypeMetadata(Value = "application/vnd.ms-project", IsBinary = true)]
        MPP,

        [ContentTypeMetadata(Value = "application/vnd.ms-project", IsBinary = true)]
        MPT,

        [ContentTypeMetadata(Value = "application/vnd.ibm.minipay", IsBinary = true)]
        MPY,

        [ContentTypeMetadata(Value = "application/vnd.mobius.mqy", IsBinary = true)]
        MQY,

        [ContentTypeMetadata(Value = "application/marc", IsBinary = true)]
        MRC,

        [ContentTypeMetadata(Value = "application/marcxml+xml", IsText = true)]
        MRCX,

        [ContentTypeMetadata(Value = "application/x-troff-ms", IsBinary = true)]
        MS,

        [ContentTypeMetadata(Value = "application/mediaservercontrol+xml", IsText = true)]
        MSCML,

        [ContentTypeMetadata(Value = "application/vnd.fdsn.mseed", IsBinary = true)]
        MSEED,

        [ContentTypeMetadata(Value = "application/vnd.mseq", IsBinary = true)]
        MSEQ,

        [ContentTypeMetadata(Value = "application/vnd.epson.msf", IsBinary = true)]
        MSF,

        [ContentTypeMetadata(Value = "model/mesh", IsBinary = true)]
        MSH,

        [ContentTypeMetadata(Value = "application/x-msdownload", IsBinary = true)]
        MSI,

        [ContentTypeMetadata(Value = "application/vnd.mobius.msl", IsBinary = true)]
        MSL,

        [ContentTypeMetadata(Value = "application/vnd.muvee.style", IsBinary = true)]
        MSTY,

        [ContentTypeMetadata(Value = "model/vnd.mts", IsBinary = true)]
        MTS,

        [ContentTypeMetadata(Value = "application/vnd.musician", IsBinary = true)]
        MUS,

        [ContentTypeMetadata(Value = "application/vnd.recordare.musicxml+xml", IsText = true)]
        MUSICXML,

        [ContentTypeMetadata(Value = "application/x-msmediaview", IsBinary = true)]
        MVB,

        [ContentTypeMetadata(Value = "application/vnd.mfer", IsBinary = true)]
        MWF,

        [ContentTypeMetadata(Value = "application/mxf", IsBinary = true)]
        MXF,

        [ContentTypeMetadata(Value = "application/vnd.recordare.musicxml", IsText = true)]
        MXL,

        [ContentTypeMetadata(Value = "application/xv+xml", IsText = true)]
        MXML,

        [ContentTypeMetadata(Value = "application/vnd.triscape.mxs", IsBinary = true)]
        MXS,

        [ContentTypeMetadata(Value = "video/vnd.mpegurl", IsBinary = true)]
        MXU,

        [ContentTypeMetadata(Value = "text/n3", IsText = true)]
        N3,

        [ContentTypeMetadata(Value = "application/mathematica", IsBinary = true)]
        NB,

        [ContentTypeMetadata(Value = "application/vnd.wolfram.player", IsBinary = true)]
        NBP,

        [ContentTypeMetadata(Value = "application/x-netcdf", IsBinary = true)]
        NC,

        [ContentTypeMetadata(Value = "application/x-dtbncx+xml", IsText = true)]
        NCX,

        [ContentTypeMetadata(Value = "text/x-nfo", IsText = true)]
        NFO,

        [ContentTypeMetadata(Value = "application/vnd.nokia.n-gage.data", IsBinary = true)]
        NGDAT,

        [ContentTypeMetadata(Value = "application/vnd.nitf", IsBinary = true)]
        NITF,

        [ContentTypeMetadata(Value = "application/vnd.neurolanguage.nlu", IsBinary = true)]
        NLU,

        [ContentTypeMetadata(Value = "application/vnd.enliven", IsBinary = true)]
        NML,

        [ContentTypeMetadata(Value = "application/vnd.noblenet-directory", IsBinary = true)]
        NND,

        [ContentTypeMetadata(Value = "application/vnd.noblenet-sealer", IsBinary = true)]
        NNS,

        [ContentTypeMetadata(Value = "application/vnd.noblenet-web", IsBinary = true)]
        NNW,

        [ContentTypeMetadata(Value = "image/vnd.net-fpx", IsBinary = true)]
        NPX,

        [ContentTypeMetadata(Value = "application/x-conference", IsBinary = true)]
        NSC,

        [ContentTypeMetadata(Value = "application/vnd.lotus-notes", IsBinary = true)]
        NSF,

        [ContentTypeMetadata(Value = "application/vnd.nitf", IsBinary = true)]
        NTF,

        [ContentTypeMetadata(Value = "application/x-nzb", IsBinary = true)]
        NZB,

        [ContentTypeMetadata(Value = "application/vnd.fujitsu.oasys2", IsBinary = true)]
        OA2,

        [ContentTypeMetadata(Value = "application/vnd.fujitsu.oasys3", IsBinary = true)]
        OA3,

        [ContentTypeMetadata(Value = "application/vnd.fujitsu.oasys", IsBinary = true)]
        OAS,

        [ContentTypeMetadata(Value = "application/x-msbinder", IsBinary = true)]
        OBD,

        [ContentTypeMetadata(Value = "application/x-tgif", IsBinary = true)]
        OBJ,

        [ContentTypeMetadata(Value = "application/oda", IsBinary = true)]
        ODA,

        [ContentTypeMetadata(Value = "application/vnd.oasis.opendocument.database", IsBinary = true)]
        ODB,

        [ContentTypeMetadata(Value = "application/vnd.oasis.opendocument.chart", IsBinary = true)]
        ODC,

        [ContentTypeMetadata(Value = "application/vnd.oasis.opendocument.formula", IsBinary = true)]
        ODF,

        [ContentTypeMetadata(Value = "application/vnd.oasis.opendocument.formula-template", IsBinary = true)]
        ODFT,

        [ContentTypeMetadata(Value = "application/vnd.oasis.opendocument.graphics", IsBinary = true)]
        ODG,

        [ContentTypeMetadata(Value = "application/vnd.oasis.opendocument.image", IsBinary = true)]
        ODI,

        [ContentTypeMetadata(Value = "application/vnd.oasis.opendocument.text-master", IsBinary = true)]
        ODM,

        [ContentTypeMetadata(Value = "application/vnd.oasis.opendocument.presentation", IsBinary = true)]
        ODP,

        [ContentTypeMetadata(Value = "application/vnd.oasis.opendocument.spreadsheet", IsBinary = true)]
        ODS,

        [ContentTypeMetadata(Value = "application/vnd.oasis.opendocument.text", IsBinary = true)]
        ODT,

        [ContentTypeMetadata(Value = "audio/ogg", IsBinary = true)]
        OGA,

        [ContentTypeMetadata(Value = "video/ogg", IsBinary = true)]
        OGG,

        [ContentTypeMetadata(Value = "video/ogg", IsBinary = true)]
        OGV,

        [ContentTypeMetadata(Value = "application/ogg", IsBinary = true)]
        OGX,

        [ContentTypeMetadata(Value = "application/omdoc+xml", IsText = true)]
        OMDOC,

        [ContentTypeMetadata(Value = "application/onenote", IsBinary = true)]
        ONEPKG,

        [ContentTypeMetadata(Value = "application/onenote", IsBinary = true)]
        ONETMP,

        [ContentTypeMetadata(Value = "application/onenote", IsBinary = true)]
        ONETOC,

        [ContentTypeMetadata(Value = "application/onenote", IsBinary = true)]
        ONETOC2,

        [ContentTypeMetadata(Value = "application/oebps-package+xml", IsText = true)]
        OPF,

        [ContentTypeMetadata(Value = "text/x-opml", IsText = true)]
        OPML,

        [ContentTypeMetadata(Value = "application/vnd.palm", IsBinary = true)]
        OPRC,

        [ContentTypeMetadata(Value = "application/vnd.lotus-organizer", IsBinary = true)]
        ORG,

        [ContentTypeMetadata(Value = "application/vnd.yamaha.openscoreformat", IsBinary = true)]
        OSF,

        [ContentTypeMetadata(Value = "application/vnd.yamaha.openscoreformat.osfpvg+xml", IsText = true)]
        OSFPVG,

        [ContentTypeMetadata(Value = "application/vnd.oasis.opendocument.chart-template", IsBinary = true)]
        OTC,

        [ContentTypeMetadata(Value = "application/x-font-otf", IsBinary = true)]
        OTF,

        [ContentTypeMetadata(Value = "application/vnd.oasis.opendocument.graphics-template", IsBinary = true)]
        OTG,

        [ContentTypeMetadata(Value = "application/vnd.oasis.opendocument.text-web", IsBinary = true)]
        OTH,

        [ContentTypeMetadata(Value = "application/vnd.oasis.opendocument.image-template", IsBinary = true)]
        OTI,

        [ContentTypeMetadata(Value = "application/vnd.oasis.opendocument.presentation-template", IsBinary = true)]
        OTP,

        [ContentTypeMetadata(Value = "application/vnd.oasis.opendocument.spreadsheet-template", IsBinary = true)]
        OTS,

        [ContentTypeMetadata(Value = "application/vnd.oasis.opendocument.text-template", IsBinary = true)]
        OTT,

        [ContentTypeMetadata(Value = "application/oxps", IsBinary = true)]
        OXPS,

        [ContentTypeMetadata(Value = "application/vnd.openofficeorg.extension", IsBinary = true)]
        OXT,

        [ContentTypeMetadata(Value = "text/x-pascal", IsText = true)]
        P,

        [ContentTypeMetadata(Value = "application/pkcs10", IsBinary = true)]
        P10,

        [ContentTypeMetadata(Value = "application/x-pkcs12", IsBinary = true)]
        P12,

        [ContentTypeMetadata(Value = "application/x-pkcs7-certificates", IsBinary = true)]
        P7B,

        [ContentTypeMetadata(Value = "application/pkcs7-mime", IsBinary = true)]
        P7C,

        [ContentTypeMetadata(Value = "application/pkcs7-mime", IsBinary = true)]
        P7M,

        [ContentTypeMetadata(Value = "application/x-pkcs7-certreqresp", IsBinary = true)]
        P7R,

        [ContentTypeMetadata(Value = "application/pkcs7-signature", IsBinary = true)]
        P7S,

        [ContentTypeMetadata(Value = "application/pkcs8", IsBinary = true)]
        P8,

        [ContentTypeMetadata(Value = "text/x-pascal", IsText = true)]
        PAS,

        [ContentTypeMetadata(Value = "application/vnd.pawaafile", IsBinary = true)]
        PAW,

        [ContentTypeMetadata(Value = "application/vnd.powerbuilder6", IsBinary = true)]
        PBD,

        [ContentTypeMetadata(Value = "image/x-portable-bitmap", IsBinary = true)]
        PBM,

        [ContentTypeMetadata(Value = "application/vnd.tcpdump.pcap", IsBinary = true)]
        PCAP,

        [ContentTypeMetadata(Value = "application/x-font-pcf", IsBinary = true)]
        PCF,

        [ContentTypeMetadata(Value = "application/vnd.hp-pcl", IsBinary = true)]
        PCL,

        [ContentTypeMetadata(Value = "application/vnd.hp-pclxl", IsBinary = true)]
        PCLXL,

        [ContentTypeMetadata(Value = "image/x-pict", IsBinary = true)]
        PCT,

        [ContentTypeMetadata(Value = "application/vnd.curl.pcurl", IsBinary = true)]
        PCURL,

        [ContentTypeMetadata(Value = "image/x-pcx", IsBinary = true)]
        PCX,

        [ContentTypeMetadata(Value = "applicaton/octet-stream", IsBinary = true)]
        PDB,

        [ContentTypeMetadata(Value = "application/pdf", IsBinary = true)]
        PDF,

        [ContentTypeMetadata(Value = "application/x-font-type1", IsBinary = true)]
        PFA,

        [ContentTypeMetadata(Value = "application/x-font-type1", IsBinary = true)]
        PFB,

        [ContentTypeMetadata(Value = "application/x-font-type1", IsBinary = true)]
        PFM,

        [ContentTypeMetadata(Value = "application/font-tdpfr", IsBinary = true)]
        PFR,

        [ContentTypeMetadata(Value = "application/x-pkcs12", IsBinary = true)]
        PFX,

        [ContentTypeMetadata(Value = "image/x-portable-graymap", IsBinary = true)]
        PGM,

        [ContentTypeMetadata(Value = "application/x-chess-pgn", IsBinary = true)]
        PGN,

        [ContentTypeMetadata(Value = "application/pgp-encrypted", IsBinary = true)]
        PGP,

        [ContentTypeMetadata(Value = "image/x-pict", IsBinary = true)]
        PIC,

        [ContentTypeMetadata(Value = "image/pict", IsBinary = true)]
        PICT,

        [ContentTypeMetadata(Value = "application/octet-stream", IsBinary = true)]
        PKG,

        [ContentTypeMetadata(Value = "application/pkixcmp", IsBinary = true)]
        PKI,

        [ContentTypeMetadata(Value = "application/pkix-pkipath", IsBinary = true)]
        PKIPATH,

        [ContentTypeMetadata(Value = "application/vnd.3gpp.pic-bw-large", IsBinary = true)]
        PLB,

        [ContentTypeMetadata(Value = "application/vnd.mobius.plc", IsBinary = true)]
        PLC,

        [ContentTypeMetadata(Value = "application/vnd.pocketlearn", IsBinary = true)]
        PLF,

        [ContentTypeMetadata(Value = "application/pls+xml", IsText = true)]
        PLS,

        [ContentTypeMetadata(Value = "application/vnd.ctc-posml", IsBinary = true)]
        PML,

        [ContentTypeMetadata(Value = "image/png", IsBinary = true)]
        PNG,

        [ContentTypeMetadata(Value = "image/x-portable-anymap", IsBinary = true)]
        PNM,

        [ContentTypeMetadata(Value = "image/x-macpaint", IsBinary = true)]
        PNT,

        [ContentTypeMetadata(Value = "image/x-macpaint", IsBinary = true)]
        PNTG,

        [ContentTypeMetadata(Value = "application/vnd.macports.portpkg", IsBinary = true)]
        PORTPKG,

        [ContentTypeMetadata(Value = "application/vnd.ms-powerpoint", IsBinary = true)]
        POT,

        [ContentTypeMetadata(Value = "application/vnd.ms-powerpoint.template.macroenabled.12", IsBinary = true)]
        POTM,

        [ContentTypeMetadata(Value = "application/vnd.openxmlformats-officedocument.presentationml.template", IsBinary = true)]
        POTX,

        [ContentTypeMetadata(Value = "application/vnd.ms-powerpoint.addin.macroenabled.12", IsBinary = true)]
        PPAM,

        [ContentTypeMetadata(Value = "application/vnd.cups-ppd", IsBinary = true)]
        PPD,

        [ContentTypeMetadata(Value = "image/x-portable-pixmap", IsBinary = true)]
        PPM,

        [ContentTypeMetadata(Value = "application/vnd.ms-powerpoint", IsBinary = true)]
        PPS,

        [ContentTypeMetadata(Value = "application/vnd.ms-powerpoint.slideshow.macroenabled.12", IsBinary = true)]
        PPSM,

        [ContentTypeMetadata(Value = "application/vnd.openxmlformats-officedocument.presentationml.slideshow", IsBinary = true)]
        PPSX,

        [ContentTypeMetadata(Value = "application/vnd.ms-powerpoint", IsBinary = true)]
        PPT,

        [ContentTypeMetadata(Value = "application/vnd.ms-powerpoint.presentation.macroenabled.12", IsBinary = true)]
        PPTM,

        [ContentTypeMetadata(Value = "application/vnd.openxmlformats-officedocument.presentationml.presentation", IsBinary = true)]
        PPTX,

        [ContentTypeMetadata(Value = "application/vnd.palm", IsBinary = true)]
        PQA,

        [ContentTypeMetadata(Value = "application/x-mobipocket-ebook", IsBinary = true)]
        PRC,

        [ContentTypeMetadata(Value = "application/vnd.lotus-freelance", IsBinary = true)]
        PRE,

        [ContentTypeMetadata(Value = "application/pics-rules", IsBinary = true)]
        PRF,

        [ContentTypeMetadata(Value = "application/postscript", IsBinary = true)]
        PS,

        [ContentTypeMetadata(Value = "application/vnd.3gpp.pic-bw-small", IsBinary = true)]
        PSB,

        [ContentTypeMetadata(Value = "image/vnd.adobe.photoshop", IsBinary = true)]
        PSD,

        [ContentTypeMetadata(Value = "application/x-font-linux-psf", IsBinary = true)]
        PSF,

        [ContentTypeMetadata(Value = "application/pskc+xml", IsText = true)]
        PSKCXML,

        [ContentTypeMetadata(Value = "application/vnd.pvi.ptid1", IsBinary = true)]
        PTID,

        [ContentTypeMetadata(Value = "application/x-mspublisher", IsBinary = true)]
        PUB,

        [ContentTypeMetadata(Value = "application/vnd.3gpp.pic-bw-var", IsBinary = true)]
        PVB,

        [ContentTypeMetadata(Value = "application/vnd.3m.post-it-notes", IsBinary = true)]
        PWN,

        [ContentTypeMetadata(Value = "audio/vnd.ms-playready.media.pya", IsBinary = true)]
        PYA,

        [ContentTypeMetadata(Value = "video/vnd.ms-playready.media.pyv", IsBinary = true)]
        PYV,

        [ContentTypeMetadata(Value = "application/vnd.epson.quickanime", IsBinary = true)]
        QAM,

        [ContentTypeMetadata(Value = "application/vnd.intu.qbo", IsBinary = true)]
        QBO,

        [ContentTypeMetadata(Value = "application/vnd.intu.qfx", IsBinary = true)]
        QFX,

        [ContentTypeMetadata(Value = "application/vnd.publishare-delta-tree", IsBinary = true)]
        QPS,

        [ContentTypeMetadata(Value = "video/quicktime", IsBinary = true)]
        QT,

        [ContentTypeMetadata(Value = "image/x-quicktime", IsBinary = true)]
        QTI,

        [ContentTypeMetadata(Value = "image/x-quicktime", IsBinary = true)]
        QTIF,

        [ContentTypeMetadata(Value = "application/vnd.quark.quarkxpress", IsBinary = true)]
        QWD,

        [ContentTypeMetadata(Value = "application/vnd.quark.quarkxpress", IsBinary = true)]
        QWT,

        [ContentTypeMetadata(Value = "application/vnd.quark.quarkxpress", IsBinary = true)]
        QXB,

        [ContentTypeMetadata(Value = "application/vnd.quark.quarkxpress", IsBinary = true)]
        QXD,

        [ContentTypeMetadata(Value = "application/vnd.quark.quarkxpress", IsBinary = true)]
        QXL,

        [ContentTypeMetadata(Value = "application/vnd.quark.quarkxpress", IsBinary = true)]
        QXT,

        [ContentTypeMetadata(Value = "audio/x-pn-realaudio", IsBinary = true)]
        RA,

        [ContentTypeMetadata(Value = "audio/x-pn-realaudio", IsBinary = true)]
        RAM,

        [ContentTypeMetadata(Value = "application/x-rar-compressed", IsBinary = true)]
        RAR,

        [ContentTypeMetadata(Value = "image/x-cmu-raster", IsBinary = true)]
        RAS,

        [ContentTypeMetadata(Value = "application/vnd.ipunplugged.rcprofile", IsBinary = true)]
        RCPROFILE,

        [ContentTypeMetadata(Value = "application/rdf+xml", IsText = true)]
        RDF,

        [ContentTypeMetadata(Value = "application/vnd.data-vision.rdz", IsBinary = true)]
        RDZ,

        [ContentTypeMetadata(Value = "application/vnd.businessobjects", IsBinary = true)]
        REP,

        [ContentTypeMetadata(Value = "application/x-dtbresource+xml", IsText = true)]
        RES,

        [ContentTypeMetadata(Value = "image/x-rgb", IsBinary = true)]
        RGB,

        [ContentTypeMetadata(Value = "application/reginfo+xml", IsText = true)]
        RIF,

        [ContentTypeMetadata(Value = "audio/vnd.rip", IsBinary = true)]
        RIP,

        [ContentTypeMetadata(Value = "application/x-research-info-systems", IsBinary = true)]
        RIS,

        [ContentTypeMetadata(Value = "application/resource-lists+xml", IsText = true)]
        RL,

        [ContentTypeMetadata(Value = "image/vnd.fujixerox.edmics-rlc", IsBinary = true)]
        RLC,

        [ContentTypeMetadata(Value = "application/resource-lists-diff+xml", IsText = true)]
        RLD,

        [ContentTypeMetadata(Value = "application/vnd.rn-realmedia", IsBinary = true)]
        RM,

        [ContentTypeMetadata(Value = "audio/midi", IsBinary = true)]
        RMI,

        [ContentTypeMetadata(Value = "audio/x-pn-realaudio-plugin", IsBinary = true)]
        RMP,

        [ContentTypeMetadata(Value = "application/vnd.jcp.javame.midlet-rms", IsBinary = true)]
        RMS,

        [ContentTypeMetadata(Value = "application/vnd.rn-realmedia-vbr", IsBinary = true)]
        RMVB,

        [ContentTypeMetadata(Value = "application/relax-ng-compact-syntax", IsBinary = true)]
        RNC,

        [ContentTypeMetadata(Value = "application/rpki-roa", IsBinary = true)]
        ROA,

        [ContentTypeMetadata(Value = "application/x-troff", IsBinary = true)]
        ROFF,

        [ContentTypeMetadata(Value = "application/vnd.cloanto.rp9", IsBinary = true)]
        RP9,

        [ContentTypeMetadata(Value = "application/vnd.nokia.radio-presets", IsBinary = true)]
        RPSS,

        [ContentTypeMetadata(Value = "application/vnd.nokia.radio-preset", IsBinary = true)]
        RPST,

        [ContentTypeMetadata(Value = "application/sparql-query", IsBinary = true)]
        RQ,

        [ContentTypeMetadata(Value = "application/rls-services+xml", IsText = true)]
        RS,

        [ContentTypeMetadata(Value = "application/rsd+xml", IsText = true)]
        RSD,

        [ContentTypeMetadata(Value = "application/rss+xml", IsText = true)]
        RSS,

        [ContentTypeMetadata(Value = "application/rtf", IsBinary = true)]
        RTF,

        [ContentTypeMetadata(Value = "text/richtext", IsText = true)]
        RTX,

        [ContentTypeMetadata(Value = "text/x-asm", IsText = true)]
        S,

        [ContentTypeMetadata(Value = "audio/s3m", IsBinary = true)]
        S3M,

        [ContentTypeMetadata(Value = "application/vnd.yamaha.smaf-audio", IsBinary = true)]
        SAF,

        [ContentTypeMetadata(Value = "application/sbml+xml", IsText = true)]
        SBML,

        [ContentTypeMetadata(Value = "application/vnd.ibm.secure-container", IsBinary = true)]
        SC,

        [ContentTypeMetadata(Value = "application/x-msschedule", IsBinary = true)]
        SCD,

        [ContentTypeMetadata(Value = "application/vnd.lotus-screencam", IsBinary = true)]
        SCM,

        [ContentTypeMetadata(Value = "application/scvp-cv-request", IsBinary = true)]
        SCQ,

        [ContentTypeMetadata(Value = "application/scvp-cv-response", IsBinary = true)]
        SCS,

        [ContentTypeMetadata(Value = "text/vnd.curl.scurl", IsText = true)]
        SCURL,

        [ContentTypeMetadata(Value = "application/vnd.stardivision.draw", IsBinary = true)]
        SDA,

        [ContentTypeMetadata(Value = "application/vnd.stardivision.calc", IsBinary = true)]
        SDC,

        [ContentTypeMetadata(Value = "application/vnd.stardivision.impress", IsBinary = true)]
        SDD,

        [ContentTypeMetadata(Value = "application/vnd.solent.sdkm+xml", IsText = true)]
        SDKD,

        [ContentTypeMetadata(Value = "application/vnd.solent.sdkm+xml", IsText = true)]
        SDKM,

        [ContentTypeMetadata(Value = "application/sdp", IsBinary = true)]
        SDP,

        [ContentTypeMetadata(Value = "application/vnd.stardivision.writer", IsBinary = true)]
        SDW,

        [ContentTypeMetadata(Value = "application/vnd.seemail", IsBinary = true)]
        SEE,

        [ContentTypeMetadata(Value = "application/vnd.fdsn.seed", IsBinary = true)]
        SEED,

        [ContentTypeMetadata(Value = "application/vnd.sema", IsBinary = true)]
        SEMA,

        [ContentTypeMetadata(Value = "application/vnd.semd", IsBinary = true)]
        SEMD,

        [ContentTypeMetadata(Value = "application/vnd.semf", IsBinary = true)]
        SEMF,

        [ContentTypeMetadata(Value = "application/java-serialized-object", IsBinary = true)]
        SER,

        [ContentTypeMetadata(Value = "application/set-payment-initiation", IsBinary = true)]
        SETPAY,

        [ContentTypeMetadata(Value = "application/set-registration-initiation", IsBinary = true)]
        SETREG,

        [ContentTypeMetadata(Value = "application/vnd.spotfire.sfs", IsBinary = true)]
        SFS,

        [ContentTypeMetadata(Value = "text/x-sfv", IsText = true)]
        SFV,

        [ContentTypeMetadata(Value = "image/sgi", IsBinary = true)]
        SGI,

        [ContentTypeMetadata(Value = "application/vnd.stardivision.writer-global", IsBinary = true)]
        SGL,

        [ContentTypeMetadata(Value = "text/sgml", IsText = true)]
        SGM,

        [ContentTypeMetadata(Value = "text/sgml", IsText = true)]
        SGML,

        [ContentTypeMetadata(Value = "application/x-sh", IsBinary = true)]
        SH,

        [ContentTypeMetadata(Value = "application/x-shar", IsBinary = true)]
        SHAR,

        [ContentTypeMetadata(Value = "application/shf+xml", IsText = true)]
        SHF,

        [ContentTypeMetadata(Value = "image/x-mrsid-image", IsBinary = true)]
        SID,

        [ContentTypeMetadata(Value = "application/pgp-signature", IsBinary = true)]
        SIG,

        [ContentTypeMetadata(Value = "audio/silk", IsBinary = true)]
        SIL,

        [ContentTypeMetadata(Value = "model/mesh", IsBinary = true)]
        SILO,

        [ContentTypeMetadata(Value = "application/vnd.symbian.install", IsBinary = true)]
        SIS,

        [ContentTypeMetadata(Value = "application/vnd.symbian.install", IsBinary = true)]
        SISX,

        [ContentTypeMetadata(Value = "application/x-stuffit", IsBinary = true)]
        SIT,

        [ContentTypeMetadata(Value = "application/x-stuffitx", IsBinary = true)]
        SITX,

        [ContentTypeMetadata(Value = "application/x-koan", IsBinary = true)]
        SKD,

        [ContentTypeMetadata(Value = "application/x-koan", IsBinary = true)]
        SKM,

        [ContentTypeMetadata(Value = "application/x-koan", IsBinary = true)]
        SKP,

        [ContentTypeMetadata(Value = "application/x-koan", IsBinary = true)]
        SKT,

        [ContentTypeMetadata(Value = "application/vnd.ms-powerpoint.slide.macroenabled.12", IsBinary = true)]
        SLDM,

        [ContentTypeMetadata(Value = "application/vnd.openxmlformats-officedocument.presentationml.slide", IsBinary = true)]
        SLDX,

        [ContentTypeMetadata(Value = "application/vnd.epson.salt", IsBinary = true)]
        SLT,

        [ContentTypeMetadata(Value = "application/vnd.stepmania.stepchart", IsBinary = true)]
        SM,

        [ContentTypeMetadata(Value = "application/vnd.stardivision.math", IsBinary = true)]
        SMF,

        [ContentTypeMetadata(Value = "application/smil+xml", IsText = true)]
        SMI,

        [ContentTypeMetadata(Value = "application/smil+xml", IsText = true)]
        SMIL,

        [ContentTypeMetadata(Value = "video/x-smv", IsBinary = true)]
        SMV,

        [ContentTypeMetadata(Value = "application/vnd.stepmania.package", IsBinary = true)]
        SMZIP,

        [ContentTypeMetadata(Value = "audio/basic", IsBinary = true)]
        SND,

        [ContentTypeMetadata(Value = "application/x-font-snf", IsBinary = true)]
        SNF,

        [ContentTypeMetadata(Value = "application/octet-stream", IsBinary = true)]
        SO,

        [ContentTypeMetadata(Value = "application/x-pkcs7-certificates", IsBinary = true)]
        SPC,

        [ContentTypeMetadata(Value = "application/vnd.yamaha.smaf-phrase", IsBinary = true)]
        SPF,

        [ContentTypeMetadata(Value = "application/x-futuresplash", IsBinary = true)]
        SPL,

        [ContentTypeMetadata(Value = "text/vnd.in3d.spot", IsText = true)]
        SPOT,

        [ContentTypeMetadata(Value = "application/scvp-vp-response", IsBinary = true)]
        SPP,

        [ContentTypeMetadata(Value = "application/scvp-vp-request", IsBinary = true)]
        SPQ,

        [ContentTypeMetadata(Value = "audio/ogg", IsBinary = true)]
        SPX,

        [ContentTypeMetadata(Value = "application/x-sql", IsBinary = true)]
        SQL,

        [ContentTypeMetadata(Value = "application/x-wais-source", IsBinary = true)]
        SRC,

        [ContentTypeMetadata(Value = "application/x-subrip", IsBinary = true)]
        SRT,

        [ContentTypeMetadata(Value = "application/sru+xml", IsText = true)]
        SRU,

        [ContentTypeMetadata(Value = "application/sparql-results+xml", IsText = true)]
        SRX,

        [ContentTypeMetadata(Value = "application/ssdl+xml", IsText = true)]
        SSDL,

        [ContentTypeMetadata(Value = "application/vnd.kodak-descriptor", IsBinary = true)]
        SSE,

        [ContentTypeMetadata(Value = "application/vnd.epson.ssf", IsBinary = true)]
        SSF,

        [ContentTypeMetadata(Value = "application/ssml+xml", IsText = true)]
        SSML,

        [ContentTypeMetadata(Value = "application/vnd.sailingtracker.track", IsBinary = true)]
        ST,

        [ContentTypeMetadata(Value = "application/vnd.sun.xml.calc.template", IsBinary = true)]
        STC,

        [ContentTypeMetadata(Value = "application/vnd.sun.xml.draw.template", IsBinary = true)]
        STD,

        [ContentTypeMetadata(Value = "application/vnd.wt.stf", IsBinary = true)]
        STF,

        [ContentTypeMetadata(Value = "application/vnd.sun.xml.impress.template", IsBinary = true)]
        STI,

        [ContentTypeMetadata(Value = "application/hyperstudio", IsBinary = true)]
        STK,

        [ContentTypeMetadata(Value = "application/vnd.ms-pki.stl", IsBinary = true)]
        STL,

        [ContentTypeMetadata(Value = "application/vnd.pg.format", IsBinary = true)]
        STR,

        [ContentTypeMetadata(Value = "application/vnd.sun.xml.writer.template", IsBinary = true)]
        STW,

        [ContentTypeMetadata(Value = "text/vnd.dvb.subtitle", IsText = true)]
        SUB,

        [ContentTypeMetadata(Value = "application/vnd.sus-calendar", IsBinary = true)]
        SUS,

        [ContentTypeMetadata(Value = "application/vnd.sus-calendar", IsBinary = true)]
        SUSP,

        [ContentTypeMetadata(Value = "application/x-sv4cpio", IsBinary = true)]
        SV4CPIO,

        [ContentTypeMetadata(Value = "application/x-sv4crc", IsBinary = true)]
        SV4CRC,

        [ContentTypeMetadata(Value = "application/vnd.dvb.service", IsBinary = true)]
        SVC,

        [ContentTypeMetadata(Value = "application/vnd.svd", IsBinary = true)]
        SVD,

        [ContentTypeMetadata(Value = "image/svg+xml", IsText = true)]
        SVG,

        [ContentTypeMetadata(Value = "image/svg+xml", IsText = true)]
        SVGZ,

        [ContentTypeMetadata(Value = "application/x-director", IsBinary = true)]
        SWA,

        [ContentTypeMetadata(Value = "application/x-shockwave-flash", IsBinary = true)]
        SWF,

        [ContentTypeMetadata(Value = "application/vnd.aristanetworks.swi", IsBinary = true)]
        SWI,

        [ContentTypeMetadata(Value = "application/vnd.sun.xml.calc", IsBinary = true)]
        SXC,

        [ContentTypeMetadata(Value = "application/vnd.sun.xml.draw", IsBinary = true)]
        SXD,

        [ContentTypeMetadata(Value = "application/vnd.sun.xml.writer.global", IsBinary = true)]
        SXG,

        [ContentTypeMetadata(Value = "application/vnd.sun.xml.impress", IsBinary = true)]
        SXI,

        [ContentTypeMetadata(Value = "application/vnd.sun.xml.math", IsBinary = true)]
        SXM,

        [ContentTypeMetadata(Value = "application/vnd.sun.xml.writer", IsBinary = true)]
        SXW,

        [ContentTypeMetadata(Value = "application/x-troff", IsBinary = true)]
        T,

        [ContentTypeMetadata(Value = "application/x-t3vm-image", IsBinary = true)]
        T3,

        [ContentTypeMetadata(Value = "application/vnd.mynfc", IsBinary = true)]
        TAGLET,

        [ContentTypeMetadata(Value = "application/vnd.tao.intent-module-archive", IsBinary = true)]
        TAO,

        [ContentTypeMetadata(Value = "application/x-tar", IsBinary = true)]
        TAR,

        [ContentTypeMetadata(Value = "application/vnd.3gpp2.tcap", IsBinary = true)]
        TCAP,

        [ContentTypeMetadata(Value = "application/x-tcl", IsBinary = true)]
        TCL,

        [ContentTypeMetadata(Value = "application/vnd.smart.teacher", IsBinary = true)]
        TEACHER,

        [ContentTypeMetadata(Value = "application/tei+xml", IsText = true)]
        TEI,

        [ContentTypeMetadata(Value = "application/tei+xml", IsText = true)]
        TEICORPUS,

        [ContentTypeMetadata(Value = "application/x-tex", IsBinary = true)]
        TEX,

        [ContentTypeMetadata(Value = "application/x-texinfo", IsBinary = true)]
        TEXI,

        [ContentTypeMetadata(Value = "application/x-texinfo", IsBinary = true)]
        TEXINFO,

        [ContentTypeMetadata(Value = "text/plain", IsText = true)]
        TEXT,

        [ContentTypeMetadata(Value = "application/thraud+xml", IsText = true)]
        TFI,

        [ContentTypeMetadata(Value = "application/x-tex-tfm", IsBinary = true)]
        TFM,

        [ContentTypeMetadata(Value = "image/x-tga", IsBinary = true)]
        TGA,

        [ContentTypeMetadata(Value = "application/vnd.ms-officetheme", IsBinary = true)]
        THMX,

        [ContentTypeMetadata(Value = "image/tiff", IsBinary = true)]
        TIF,

        [ContentTypeMetadata(Value = "image/tiff", IsBinary = true)]
        TIFF,

        [ContentTypeMetadata(Value = "application/vnd.tmobile-livetv", IsBinary = true)]
        TMO,

        [ContentTypeMetadata(Value = "application/x-bittorrent", IsBinary = true)]
        TORRENT,

        [ContentTypeMetadata(Value = "application/vnd.groove-tool-template", IsBinary = true)]
        TPL,

        [ContentTypeMetadata(Value = "application/vnd.trid.tpt", IsBinary = true)]
        TPT,

        [ContentTypeMetadata(Value = "application/x-troff", IsBinary = true)]
        TR,

        [ContentTypeMetadata(Value = "application/vnd.trueapp", IsBinary = true)]
        TRA,

        [ContentTypeMetadata(Value = "application/x-msterminal", IsBinary = true)]
        TRM,

        [ContentTypeMetadata(Value = "application/timestamped-data", IsBinary = true)]
        TSD,

        [ContentTypeMetadata(Value = "text/tab-separated-values", IsText = true)]
        TSV,

        [ContentTypeMetadata(Value = "application/x-font-ttf", IsBinary = true)]
        TTC,

        [ContentTypeMetadata(Value = "application/x-font-ttf", IsBinary = true)]
        TTF,

        [ContentTypeMetadata(Value = "text/turtle", IsText = true)]
        TTL,

        [ContentTypeMetadata(Value = "application/vnd.simtech-mindmapper", IsBinary = true)]
        TWD,

        [ContentTypeMetadata(Value = "application/vnd.simtech-mindmapper", IsBinary = true)]
        TWDS,

        [ContentTypeMetadata(Value = "application/vnd.genomatix.tuxedo", IsBinary = true)]
        TXD,

        [ContentTypeMetadata(Value = "application/vnd.mobius.txf", IsBinary = true)]
        TXF,

        [ContentTypeMetadata(Value = "text/plain", IsText = true)]
        TXT,

        [ContentTypeMetadata(Value = "application/x-authorware-bin", IsBinary = true)]
        U32,

        [ContentTypeMetadata(Value = "application/x-debian-package", IsBinary = true)]
        UDEB,

        [ContentTypeMetadata(Value = "application/vnd.ufdl", IsBinary = true)]
        UFD,

        [ContentTypeMetadata(Value = "application/vnd.ufdl", IsBinary = true)]
        UFDL,

        [ContentTypeMetadata(Value = "application/x-glulx", IsBinary = true)]
        ULX,

        [ContentTypeMetadata(Value = "application/vnd.umajin", IsBinary = true)]
        UMJ,

        [ContentTypeMetadata(Value = "application/vnd.unity", IsBinary = true)]
        UNITYWEB,

        [ContentTypeMetadata(Value = "application/vnd.uoml+xml", IsText = true)]
        UOML,

        [ContentTypeMetadata(Value = "text/uri-list", IsText = true)]
        URI,

        [ContentTypeMetadata(Value = "text/uri-list", IsText = true)]
        URIS,

        [ContentTypeMetadata(Value = "text/uri-list", IsText = true)]
        URLS,

        [ContentTypeMetadata(Value = "application/x-ustar", IsBinary = true)]
        USTAR,

        [ContentTypeMetadata(Value = "application/vnd.uiq.theme", IsBinary = true)]
        UTZ,

        [ContentTypeMetadata(Value = "text/x-uuencode", IsText = true)]
        UU,

        [ContentTypeMetadata(Value = "audio/vnd.dece.audio", IsBinary = true)]
        UVA,

        [ContentTypeMetadata(Value = "application/vnd.dece.data", IsBinary = true)]
        UVD,

        [ContentTypeMetadata(Value = "application/vnd.dece.data", IsBinary = true)]
        UVF,

        [ContentTypeMetadata(Value = "image/vnd.dece.graphic", IsBinary = true)]
        UVG,

        [ContentTypeMetadata(Value = "video/vnd.dece.hd", IsBinary = true)]
        UVH,

        [ContentTypeMetadata(Value = "image/vnd.dece.graphic", IsBinary = true)]
        UVI,

        [ContentTypeMetadata(Value = "video/vnd.dece.mobile", IsBinary = true)]
        UVM,

        [ContentTypeMetadata(Value = "video/vnd.dece.pd", IsBinary = true)]
        UVP,

        [ContentTypeMetadata(Value = "video/vnd.dece.sd", IsBinary = true)]
        UVS,

        [ContentTypeMetadata(Value = "application/vnd.dece.ttml+xml", IsText = true)]
        UVT,

        [ContentTypeMetadata(Value = "video/vnd.uvvu.mp4", IsBinary = true)]
        UVU,

        [ContentTypeMetadata(Value = "video/vnd.dece.video", IsBinary = true)]
        UVV,

        [ContentTypeMetadata(Value = "audio/vnd.dece.audio", IsBinary = true)]
        UVVA,

        [ContentTypeMetadata(Value = "application/vnd.dece.data", IsBinary = true)]
        UVVD,

        [ContentTypeMetadata(Value = "application/vnd.dece.data", IsBinary = true)]
        UVVF,

        [ContentTypeMetadata(Value = "image/vnd.dece.graphic", IsBinary = true)]
        UVVG,

        [ContentTypeMetadata(Value = "video/vnd.dece.hd", IsBinary = true)]
        UVVH,

        [ContentTypeMetadata(Value = "image/vnd.dece.graphic", IsBinary = true)]
        UVVI,

        [ContentTypeMetadata(Value = "video/vnd.dece.mobile", IsBinary = true)]
        UVVM,

        [ContentTypeMetadata(Value = "video/vnd.dece.pd", IsBinary = true)]
        UVVP,

        [ContentTypeMetadata(Value = "video/vnd.dece.sd", IsBinary = true)]
        UVVS,

        [ContentTypeMetadata(Value = "application/vnd.dece.ttml+xml", IsText = true)]
        UVVT,

        [ContentTypeMetadata(Value = "video/vnd.uvvu.mp4", IsBinary = true)]
        UVVU,

        [ContentTypeMetadata(Value = "video/vnd.dece.video", IsBinary = true)]
        UVVV,

        [ContentTypeMetadata(Value = "application/vnd.dece.unspecified", IsBinary = true)]
        UVVX,

        [ContentTypeMetadata(Value = "application/vnd.dece.zip", IsBinary = true)]
        UVVZ,

        [ContentTypeMetadata(Value = "application/vnd.dece.unspecified", IsBinary = true)]
        UVX,

        [ContentTypeMetadata(Value = "application/vnd.dece.zip", IsBinary = true)]
        UVZ,

        [ContentTypeMetadata(Value = "text/vcard", IsText = true)]
        VCARD,

        [ContentTypeMetadata(Value = "application/x-cdlink", IsBinary = true)]
        VCD,

        [ContentTypeMetadata(Value = "text/x-vcard", IsText = true)]
        VCF,

        [ContentTypeMetadata(Value = "application/vnd.groove-vcard", IsBinary = true)]
        VCG,

        [ContentTypeMetadata(Value = "text/x-vcalendar", IsText = true)]
        VCS,

        [ContentTypeMetadata(Value = "application/vnd.vcx", IsBinary = true)]
        VCX,

        [ContentTypeMetadata(Value = "application/vnd.visionary", IsBinary = true)]
        VIS,

        [ContentTypeMetadata(Value = "video/vnd.vivo", IsBinary = true)]
        VIV,

        [ContentTypeMetadata(Value = "video/x-ms-vob", IsBinary = true)]
        VOB,

        [ContentTypeMetadata(Value = "application/vnd.stardivision.writer", IsBinary = true)]
        VOR,

        [ContentTypeMetadata(Value = "application/x-authorware-bin", IsBinary = true)]
        VOX,

        [ContentTypeMetadata(Value = "model/vrml", IsBinary = true)]
        VRML,

        [ContentTypeMetadata(Value = "application/vnd.visio", IsBinary = true)]
        VSD,

        [ContentTypeMetadata(Value = "application/vnd.vsf", IsBinary = true)]
        VSF,

        [ContentTypeMetadata(Value = "application/vnd.visio", IsBinary = true)]
        VSS,

        [ContentTypeMetadata(Value = "application/vnd.visio", IsBinary = true)]
        VST,

        [ContentTypeMetadata(Value = "application/vnd.visio", IsBinary = true)]
        VSW,

        [ContentTypeMetadata(Value = "model/vnd.vtu", IsBinary = true)]
        VTU,

        [ContentTypeMetadata(Value = "application/voicexml+xml", IsText = true)]
        VXML,

        [ContentTypeMetadata(Value = "application/x-director", IsBinary = true)]
        W3D,

        [ContentTypeMetadata(Value = "application/x-doom", IsBinary = true)]
        WAD,

        [ContentTypeMetadata(Value = "audio/x-wav", IsBinary = true)]
        WAV,

        [ContentTypeMetadata(Value = "audio/x-ms-wax", IsBinary = true)]
        WAX,

        [ContentTypeMetadata(Value = "image/vnd.wap.wbmp", IsBinary = true)]
        WBMP,

        [ContentTypeMetadata(Value = "application/vnd.wap.wbxml", IsText = true)]
        WBMXL,

        [ContentTypeMetadata(Value = "application/vnd.criticaltools.wbs+xml", IsText = true)]
        WBS,

        [ContentTypeMetadata(Value = "application/vnd.wap.wbxml", IsText = true)]
        WBXML,

        [ContentTypeMetadata(Value = "application/vnd.ms-works", IsBinary = true)]
        WCM,

        [ContentTypeMetadata(Value = "application/vnd.ms-works", IsBinary = true)]
        WDB,

        [ContentTypeMetadata(Value = "image/vnd.ms-photo", IsBinary = true)]
        WDP,

        [ContentTypeMetadata(Value = "audio/webm", IsBinary = true)]
        WEBA,

        [ContentTypeMetadata(Value = "video/webm", IsBinary = true)]
        WEBM,

        [ContentTypeMetadata(Value = "image/webp", IsBinary = true)]
        WEBP,

        [ContentTypeMetadata(Value = "application/vnd.pmi.widget", IsBinary = true)]
        WG,

        [ContentTypeMetadata(Value = "application/widget", IsBinary = true)]
        WGT,

        [ContentTypeMetadata(Value = "application/vnd.ms-works", IsBinary = true)]
        WKS,

        [ContentTypeMetadata(Value = "video/x-ms-wm", IsBinary = true)]
        WM,

        [ContentTypeMetadata(Value = "audio/x-ms-wma", IsBinary = true)]
        WMA,

        [ContentTypeMetadata(Value = "application/x-ms-wmd", IsBinary = true)]
        WMD,

        [ContentTypeMetadata(Value = "application/x-msmetafile", IsBinary = true)]
        WMF,

        [ContentTypeMetadata(Value = "text/vnd.wap.wml", IsText = true)]
        WML,

        [ContentTypeMetadata(Value = "application/vnd.wap.wmlc", IsBinary = true)]
        WMLC,

        [ContentTypeMetadata(Value = "text/vnd.wap.wmlscript", IsText = true)]
        WMLS,

        [ContentTypeMetadata(Value = "application/vnd.wap.wmlscriptc", IsBinary = true)]
        WMLSC,

        [ContentTypeMetadata(Value = "video/x-ms-wmv", IsBinary = true)]
        WMV,

        [ContentTypeMetadata(Value = "video/x-ms-wmx", IsBinary = true)]
        WMX,

        [ContentTypeMetadata(Value = "application/x-msmetafile", IsBinary = true)]
        WMZ,

        [ContentTypeMetadata(Value = "application/font-woff", IsBinary = true)]
        WOFF,

        [ContentTypeMetadata(Value = "application/vnd.wordperfect", IsBinary = true)]
        WPD,

        [ContentTypeMetadata(Value = "application/vnd.ms-wpl", IsBinary = true)]
        WPL,

        [ContentTypeMetadata(Value = "application/vnd.ms-works", IsBinary = true)]
        WPS,

        [ContentTypeMetadata(Value = "application/vnd.wqd", IsBinary = true)]
        WQD,

        [ContentTypeMetadata(Value = "application/x-mswrite", IsBinary = true)]
        WRI,

        [ContentTypeMetadata(Value = "model/vrml", IsBinary = true)]
        WRL,

        [ContentTypeMetadata(Value = "application/wsdl+xml", IsText = true)]
        WSDL,

        [ContentTypeMetadata(Value = "application/wspolicy+xml", IsText = true)]
        WSPOLICY,

        [ContentTypeMetadata(Value = "application/vnd.webturbo", IsBinary = true)]
        WTB,

        [ContentTypeMetadata(Value = "video/x-ms-wvx", IsBinary = true)]
        WVX,

        [ContentTypeMetadata(Value = "application/x-authorware-bin", IsBinary = true)]
        X32,

        [ContentTypeMetadata(Value = "model/x3d+xml", IsText = true)]
        X3D,

        [ContentTypeMetadata(Value = "model/x3d+binary", IsBinary = true)]
        X3DB,

        [ContentTypeMetadata(Value = "model/x3d+binary", IsBinary = true)]
        X3DBZ,

        [ContentTypeMetadata(Value = "model/x3d+vrml", IsBinary = true)]
        X3DV,

        [ContentTypeMetadata(Value = "model/x3d+vrml", IsBinary = true)]
        X3DVZ,

        [ContentTypeMetadata(Value = "model/x3d+xml", IsText = true)]
        X3DZ,

        [ContentTypeMetadata(Value = "application/xaml+xml", IsText = true)]
        XAML,

        [ContentTypeMetadata(Value = "application/x-silverlight-app", IsBinary = true)]
        XAP,

        [ContentTypeMetadata(Value = "application/vnd.xara", IsBinary = true)]
        XAR,

        [ContentTypeMetadata(Value = "application/x-ms-xbap", IsBinary = true)]
        XBAP,

        [ContentTypeMetadata(Value = "application/vnd.fujixerox.docuworks.binder", IsBinary = true)]
        XBD,

        [ContentTypeMetadata(Value = "image/x-xbitmap", IsBinary = true)]
        XBM,

        [ContentTypeMetadata(Value = "application/xcap-diff+xml", IsText = true)]
        XDF,

        [ContentTypeMetadata(Value = "application/vnd.syncml.dm+xml", IsText = true)]
        XDM,

        [ContentTypeMetadata(Value = "application/vnd.adobe.xdp+xml", IsText = true)]
        XDP,

        [ContentTypeMetadata(Value = "application/dssc+xml", IsText = true)]
        XDSSC,

        [ContentTypeMetadata(Value = "application/vnd.fujixerox.docuworks", IsBinary = true)]
        XDW,

        [ContentTypeMetadata(Value = "application/xenc+xml", IsText = true)]
        XENC,

        [ContentTypeMetadata(Value = "application/patch-ops-error+xml", IsText = true)]
        XER,

        [ContentTypeMetadata(Value = "application/vnd.adobe.xfdf", IsBinary = true)]
        XFDF,

        [ContentTypeMetadata(Value = "application/vnd.xfdl", IsBinary = true)]
        XFDL,

        [ContentTypeMetadata(Value = "application/xhtml+xml", IsText = true)]
        XHT,

        [ContentTypeMetadata(Value = "application/xhtml+xml", IsText = true)]
        XHTML,

        [ContentTypeMetadata(Value = "application/xv+xml", IsText = true)]
        XHVML,

        [ContentTypeMetadata(Value = "image/vnd.xiff", IsBinary = true)]
        XIF,

        [ContentTypeMetadata(Value = "application/vnd.ms-excel", IsBinary = true)]
        XLA,

        [ContentTypeMetadata(Value = "application/vnd.ms-excel.addin.macroenabled.12", IsBinary = true)]
        XLAM,

        [ContentTypeMetadata(Value = "application/vnd.ms-excel", IsBinary = true)]
        XLC,

        [ContentTypeMetadata(Value = "application/x-xliff+xml", IsText = true)]
        XLF,

        [ContentTypeMetadata(Value = "application/vnd.ms-excel", IsBinary = true)]
        XLM,

        [ContentTypeMetadata(Value = "application/vnd.ms-excel", IsBinary = true)]
        XLS,

        [ContentTypeMetadata(Value = "application/vnd.ms-excel.sheet.binary.macroenabled.12", IsBinary = true)]
        XLSB,

        [ContentTypeMetadata(Value = "application/vnd.ms-excel.sheet.macroenabled.12", IsBinary = true)]
        XLSM,

        [ContentTypeMetadata(Value = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", IsBinary = true)]
        XLSX,

        [ContentTypeMetadata(Value = "application/vnd.ms-excel", IsBinary = true)]
        XLT,

        [ContentTypeMetadata(Value = "application/vnd.ms-excel.template.macroenabled.12", IsBinary = true)]
        XLTM,

        [ContentTypeMetadata(Value = "application/vnd.openxmlformats-officedocument.spreadsheetml.template", IsBinary = true)]
        XLTX,

        [ContentTypeMetadata(Value = "application/vnd.ms-excel", IsBinary = true)]
        XLW,

        [ContentTypeMetadata(Value = "audio/xm", IsBinary = true)]
        XM,

        [ContentTypeMetadata(Value = "application/xml", IsText = true)]
        XML,

        [ContentTypeMetadata(Value = "application/vnd.olpc-sugar", IsBinary = true)]
        XO,

        [ContentTypeMetadata(Value = "application/xop+xml", IsText = true)]
        XOP,

        [ContentTypeMetadata(Value = "application/x-xpinstall", IsBinary = true)]
        XPI,

        [ContentTypeMetadata(Value = "application/xproc+xml", IsText = true)]
        XPL,

        [ContentTypeMetadata(Value = "image/x-xpixmap", IsBinary = true)]
        XPM,

        [ContentTypeMetadata(Value = "application/vnd.is-xpr", IsBinary = true)]
        XPR,

        [ContentTypeMetadata(Value = "application/vnd.ms-xpsdocument", IsBinary = true)]
        XPS,

        [ContentTypeMetadata(Value = "application/vnd.intercon.formnet", IsBinary = true)]
        XPW,

        [ContentTypeMetadata(Value = "application/vnd.intercon.formnet", IsBinary = true)]
        XPX,

        [ContentTypeMetadata(Value = "application/xml", IsText = true)]
        XSL,

        [ContentTypeMetadata(Value = "application/xslt+xml", IsText = true)]
        XSLT,

        [ContentTypeMetadata(Value = "application/vnd.syncml+xml", IsText = true)]
        XSM,

        [ContentTypeMetadata(Value = "application/xspf+xml", IsText = true)]
        XSPF,

        [ContentTypeMetadata(Value = "application/vnd.mozilla.xul+xml", IsText = true)]
        XUL,

        [ContentTypeMetadata(Value = "application/xv+xml", IsText = true)]
        XVM,

        [ContentTypeMetadata(Value = "application/xv+xml", IsText = true)]
        XVML,

        [ContentTypeMetadata(Value = "image/x-xwindowdump", IsBinary = true)]
        XWD,

        [ContentTypeMetadata(Value = "chemical/x-xyz", IsBinary = true)]
        XYZ,

        [ContentTypeMetadata(Value = "application/x-xz", IsBinary = true)]
        XZ,

        [ContentTypeMetadata(Value = "application/yang", IsBinary = true)]
        YANG,

        [ContentTypeMetadata(Value = "application/yin+xml", IsText = true)]
        YIN,

        [ContentTypeMetadata(Value = "application/x-zmachine", IsBinary = true)]
        Z1,

        [ContentTypeMetadata(Value = "application/x-zmachine", IsBinary = true)]
        Z2,

        [ContentTypeMetadata(Value = "application/x-zmachine", IsBinary = true)]
        Z3,

        [ContentTypeMetadata(Value = "application/x-zmachine", IsBinary = true)]
        Z4,

        [ContentTypeMetadata(Value = "application/x-zmachine", IsBinary = true)]
        Z5,

        [ContentTypeMetadata(Value = "application/x-zmachine", IsBinary = true)]
        Z6,

        [ContentTypeMetadata(Value = "application/x-zmachine", IsBinary = true)]
        Z7,

        [ContentTypeMetadata(Value = "application/x-zmachine", IsBinary = true)]
        Z8,

        [ContentTypeMetadata(Value = "application/vnd.zzazz.deck+xml", IsText = true)]
        ZAZ,

        [ContentTypeMetadata(Value = "application/zip", IsBinary = true)]
        ZIP,

        [ContentTypeMetadata(Value = "application/vnd.zul", IsBinary = true)]
        ZIR,

        [ContentTypeMetadata(Value = "application/vnd.zul", IsBinary = true)]
        ZIRZ,

        [ContentTypeMetadata(Value = "application/vnd.handheld-entertainment+xml", IsText = true)]
        ZMM,

        [ContentTypeMetadata(Value = "application/octet-stream", IsBinary = true)]
        DEFAULT
    }

    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    class ContentTypeMetadata : Attribute
    {
        public ContentTypeMetadata()
        {
            this.Value = "text/plain";
            this.IsText = true;
        }

        public string Value { get; set; }
        public bool IsText { get; set; }
        public bool IsBinary
        {
            get
            {
                return !this.IsText;
            }
            set
            {
                this.IsText = !value;
            }
        }
    }


    public static class ContentTypeExtensions
    {
        private static object GetMetadata(ContentType ct)
        {
            var type = ct.GetType();
            MemberInfo[] info = type.GetMember(ct.ToString());
            if ((!Object.ReferenceEquals(info, null)) && (info.Length > 0))
            {
                object[] attrs = info[0].GetCustomAttributes(typeof(ContentTypeMetadata), false);
                if ((!Object.ReferenceEquals(attrs, null)) && (attrs.Length > 0))
                {
                    return attrs[0];
                }
            }
            return null;
        }

        public static string ToValue(this ContentType ct)
        {
            var metadata = GetMetadata(ct);
            return (!Object.ReferenceEquals(metadata, null)) ? ((ContentTypeMetadata)metadata).Value : ct.ToString();
        }

        public static bool IsText(this ContentType ct)
        {
            var metadata = GetMetadata(ct);
            return (!Object.ReferenceEquals(metadata, null)) ? ((ContentTypeMetadata)metadata).IsText : true;
        }

        public static bool IsBinary(this ContentType ct)
        {
            var metadata = GetMetadata(ct);
            return (!Object.ReferenceEquals(metadata, null)) ? ((ContentTypeMetadata)metadata).IsBinary : false;
        }
    }
}
