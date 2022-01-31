﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

using Prime.Models;

namespace Prime.Migrations
{
    public partial class AddPharmacyTechnicianToa : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AgreementVersion",
                columns: new[] { "CreatedUserId", "CreatedTimeStamp", "UpdatedUserId", "UpdatedTimeStamp", "Text", "AgreementType", "EffectiveDate" },
                values: new object[] { Guid.Empty, DateTimeOffset.Parse("2019-09-16 -7:00"), Guid.Empty, DateTimeOffset.Parse("2019-09-16 -7:00"),
                "<h1>PHARMANET TERMS OF ACCESS FOR PHARMACY TECHNICIANS</h1>\r\n\r\n<p class=\"bold\">\r\n  By enrolling for access to PharmaNet, you agree to the following terms (the “Agreement”). Please read them carefully.\r\n</p>\r\n\r\n<ol>\r\n  <li>\r\n\r\n    <p class=\"bold underline\">\r\n      BACKGROUND\r\n    </p>\r\n\r\n    <p>\r\n      The Province owns and is responsible for the operation of PharmaNet, the province-wide\r\n      network that links B.C. pharmacies to a central data system. Every prescription dispensed\r\n      in community pharmacies in B.C. is entered into PharmaNet.\r\n    </p>\r\n\r\n    <p>\r\n      The purpose of providing you with access to PharmaNet is to enhance patient care by\r\n      providing timely and relevant information to persons involved in the provision of direct\r\n      patient care.\r\n    </p>\r\n\r\n    <p class=\"bold underline\">\r\n      PharmaNet contains highly sensitive confidential information, including Personal\r\n      Information and the proprietary and confidential information of third-party licensors to\r\n      the Province, and it is in the public interest to ensure that appropriate measures are in\r\n      place to protect the confidentiality of all such information. All access to and use of\r\n      PharmaNet and PharmaNet Data is subject to the Act and Privacy Laws.\r\n    </p>\r\n\r\n  </li>\r\n  <li>\r\n\r\n    <p class=\"bold underline\">\r\n      INTERPRETATION\r\n    </p>\r\n\r\n    <ol type=\"a\">\r\n      <li>\r\n\r\n        <p>\r\n          <strong>Definitions.</strong> Unless otherwise provided in this Agreement, capitalized terms will\r\n          have the meanings given below:\r\n        </p>\r\n\r\n        <ul>\r\n          <li>\r\n            <strong>“Act”</strong> means the <i>Pharmaceutical Services Act</i>.\r\n          </li>\r\n          <li>\r\n            <strong>“Approved Practice Site”</strong> means a location within which you are directly\r\n            providing Health Services, devices or related services to the person in\r\n            respect of whom PharmaNet is being accessed and which is approved by\r\n            the Province for PharmaNet access.\r\n          </li>\r\n          <li>\r\n            <strong>“Approved SSO”</strong> means a software support organization approved by the\r\n            Province that provides you with the information technology software\r\n            and/or services through which you access PharmaNet.\r\n          </li>\r\n          <li>\r\n            <strong>“Authorized Technician”</strong> means an “authorized technician” as defined in\r\n            the Information Management Regulation.\r\n          </li>\r\n          <li>\r\n            <strong>“Claim”</strong> means a claim made under the Act for payment in respect of a\r\n            benefit under the Act.\r\n          </li>\r\n          <li>\r\n\r\n            <p>\r\n              <strong>“Conformance Standards”</strong> means the following documents published by\r\n              the Province, as amended from time to time:\r\n            </p>\r\n\r\n            <ol type=\"i\">\r\n              <li>\r\n                PharmaNet Professional and Software Conformance Standards\r\n                <br>\r\n                <a href=\"https://www2.gov.bc.ca/gov/content/health/practitioner-professional-resources/software/conformance-standards\" target=\"_blank\" rel=\"noopener noreferrer\">https://www2.gov.bc.ca/gov/content/health/practitioner-professional-resources/software/conformance-standards</a>\r\n                ; and\r\n              </li>\r\n              <li>\r\n                Office of the Chief Information Officer: “Submission for Technical\r\n                Security Standard and High Level Architecture for Wireless Local\r\n                Area Network Connectivity”.\r\n                <br>\r\n                <a href=\"https://www2.gov.bc.ca/gov/content/health/practitioner-professional-resources/system-access/requirements-for-wireless-access-to-pharmanet\" target=\"_blank\" rel=\"noopener noreferrer\">https://www2.gov.bc.ca/gov/content/health/practitioner-professional-resources/system-access/requirements-for-wireless-access-to-pharmanet</a>\r\n              </li>\r\n            </ol>\r\n\r\n          </li>\r\n          <li>\r\n            <strong>“Device Provider Agent”</strong> means a person enrolled under section 11 of\r\n            the Act in the class of provider known as “device provider”.\r\n          </li>\r\n          <li>\r\n            <strong>“Grant Holder”</strong> means a person permitted access to PharmaNet who has\r\n            been issued a “grant” as defined in the Information Management\r\n            Regulation.\r\n          </li>\r\n          <li>\r\n            <strong>“Health Services”</strong> means “health services” as defined in the Information\r\n            Management Regulation.\r\n          </li>\r\n          <li>\r\n            <strong>“Information Management Regulation”</strong> means the <i>Information\r\n              Management Regulation</i>, B.C. Reg. 328/2021.\r\n          </li>\r\n          <li>\r\n            <strong>“Personal Information”</strong> means all recorded information that is about an\r\n            identifiable individual or is defined as, or deemed to be, “personal\r\n            information” or “personal health information” pursuant to any Privacy\r\n            Laws.\r\n          </li>\r\n          <li>\r\n            <strong>“PharmaCare Newsletter”</strong> means the PharmaCare newsletter published\r\n            by the Province on the following website (or such other website as may be\r\n            specified by the Province from time to time for this purpose):\r\n\r\n            <br>\r\n\r\n            <a href=\"http://www.gov.bc.ca/pharmacarenewsletter\" target=\"_blank\" rel=\"noopener noreferrer\">www.gov.bc.ca/pharmacarenewsletter</a>\r\n          </li>\r\n          <li>\r\n            <strong>“PharmaNet”</strong> means “PharmaNet” as defined in the Information\r\n            Management Regulation.\r\n          </li>\r\n          <li>\r\n            <strong>“PharmaNet Data”</strong> includes any record or information contained in\r\n            PharmaNet and any record or information in your custody, control or\r\n            possession obtained through your access to PharmaNet.\r\n          </li>\r\n          <li>\r\n            <strong>“PRIME”</strong> means the online service provided by the Province that allows\r\n            users to apply for, and manage, their access to PharmaNet, and through\r\n            which users are granted access by the Province.\r\n          </li>\r\n          <li>\r\n            <strong>“Privacy Laws”</strong> means the Act, the <i>Freedom of Information and\r\n              Protection of Privacy Act</i>, the Personal Information Protection Act, and\r\n            any other statutory or legal obligations of privacy owed by you or the\r\n            Province, whether arising under statute, by contract or at common law.\r\n          </li>\r\n          <li>\r\n            <strong>“Provider”</strong> means a person enrolled under section 11 of the Act for the\r\n            purpose of receiving payment for providing benefits.\r\n          </li>\r\n          <li>\r\n            <strong>“Provider Regulation”</strong> means the <i>Provider Regulation</i>, B.C. Reg.\r\n            222/2014.\r\n          </li>\r\n          <li>\r\n            <strong>“Province”</strong> means Her Majesty the Queen in Right of British Columbia, as represented by the\r\n            Minister of Health.\r\n          </li>\r\n          <li>\r\n            <strong>“Professional College”</strong> is the regulatory body governing your provision\r\n            of Health Services.\r\n          </li>\r\n          <li>\r\n            <strong>“Unauthorized Person”</strong> means any person other than a Grant Holder or\r\n            an Authorized Technician.\r\n          </li>\r\n        </ul>\r\n\r\n      </li>\r\n      <li>\r\n        <strong>Reference to Enactments.</strong> Unless otherwise specified, a reference to a statute or\r\n        regulation by name means the statute or regulation of British Columbia of that name, as amended or replaced from\r\n        time to time, and includes any enactment\r\n        made under the authority of that statute or regulation.\r\n      </li>\r\n      <li>\r\n\r\n        <p>\r\n          <strong>Conflicting Provisions.</strong> In the event of a conflict among provisions of this\r\n          Agreement:\r\n        </p>\r\n\r\n        <ol type=\"i\">\r\n          <li>\r\n            a provision in the body of this Agreement will prevail over any conflicting\r\n            provision in any further limits or conditions communicated to you in\r\n            writing by the Province, unless the conflicting provision expressly states\r\n            otherwise; and\r\n          </li>\r\n          <li>\r\n            a provision referred to in (i) above will prevail over any conflicting\r\n            provision in the Conformance Standards.\r\n          </li>\r\n        </ol>\r\n\r\n      </li>\r\n    </ol>\r\n\r\n  </li>\r\n  <li>\r\n\r\n    <p class=\"bold underline\">\r\n      APPLICATION OF LEGISLATION\r\n    </p>\r\n\r\n    <p>\r\n      You will strictly comply with the Act, the Information Management Regulation and all\r\n      Privacy Laws applicable to PharmaNet and PharmaNet Data.\r\n    </p>\r\n\r\n  </li>\r\n  <li>\r\n\r\n    <p class=\"bold underline\">\r\n      NOTICE THAT SPECIFIC PROVISIONS OF THE ACT APPLY DIRECTLY TO YOU\r\n    </p>\r\n\r\n    <p>\r\n      You acknowledge that:\r\n    </p>\r\n\r\n    <ol type=\"a\">\r\n      <li>\r\n        PharmaNet Data accessed by you is disclosed to you by the Province under the\r\n        authority of the Act;\r\n      </li>\r\n      <li>\r\n        specific provisions of the Act (including but not limited to sections 24, 25 and 29)\r\n        and the Information Management Regulation apply directly to you as a result; and\r\n      </li>\r\n      <li>\r\n        this Agreement documents limits and conditions, set by the minister in writing,\r\n        that the Act requires you to comply with.\r\n      </li>\r\n    </ol>\r\n\r\n  </li>\r\n  <li>\r\n\r\n    <p class=\"bold underline\">\r\n      ACCESS\r\n    </p>\r\n\r\n    <ol type=\"a\">\r\n      <li>\r\n        <strong>Grant of Access.</strong> The Province will provide you with access to PharmaNet\r\n        subject to your compliance with the limits and conditions set out in this\r\n        Agreement. The Province may from time to time, at its discretion, amend or\r\n        change the scope of your access privileges to PharmaNet as privacy, security,\r\n        business and clinical practice requirements change. In such circumstances, the\r\n        Province will use reasonable efforts to notify you of such changes.\r\n      </li>\r\n      <li>\r\n\r\n        <p>\r\n          <strong>Requirements for Access.</strong> The following requirements apply to your access to\r\n          PharmaNet:\r\n        </p>\r\n\r\n        <ol type=\"i\">\r\n          <li>\r\n            you will only access PharmaNet: at an Approved Practice Site, and using\r\n            only the technologies and applications approved by the Province;\r\n          </li>\r\n          <li>\r\n            you will not submit Claims on PharmaNet other than from an Approved\r\n            Practice Site in respect of which a person is enrolled as a Provider;\r\n          </li>\r\n          <li>\r\n            subject to section 6(b) of this Agreement, you will not use PharmaNet\r\n            Data for the purposes of quality improvement, evaluation, health care\r\n            planning, surveillance, research or other secondary uses, and will only use\r\n            PharmaNet Data for your provision of Health Services;\r\n          </li>\r\n          <li>\r\n            you will not permit any Unauthorized Person to access PharmaNet, and\r\n            you will take all reasonable measures to ensure that no Unauthorized\r\n            Person can access PharmaNet;\r\n          </li>\r\n          <li>\r\n            you will complete any training program(s) that your Approved SSO makes\r\n            available to you in relation to PharmaNet;\r\n          </li>\r\n          <li>\r\n            you will comply with any additional limits or conditions applicable to you,\r\n            as may be communicated to you by the Province in writing.\r\n          </li>\r\n        </ol>\r\n      </li>\r\n      <li>\r\n\r\n        <p>\r\n          <strong>Privacy and Security Measures.</strong> You will take all reasonable measures to\r\n          safeguard Personal Information, including any Personal Information in\r\n          PharmaNet Data that is in your custody, control or possession.. In particular, you\r\n          will:\r\n        </p>\r\n\r\n        <ol type=\"i\">\r\n          <li>\r\n            take all reasonable steps to ensure the physical security of Personal\r\n            Information, generally and as required by Privacy Laws;\r\n          </li>\r\n          <li>\r\n            secure any workstations used to access PharmaNet and all devices, codes\r\n            or passwords that enable access to PharmaNet;\r\n          </li>\r\n          <li>\r\n            take such other privacy and security measures as the Province may\r\n            reasonably require from time-to-time.\r\n          </li>\r\n        </ol>\r\n\r\n      </li>\r\n      <li>\r\n        <strong>Conformance Standards.</strong> You will comply with the rules specified in the\r\n        Conformance Standards when accessing and recording information in PharmaNet.\r\n      </li>\r\n    </ol>\r\n\r\n  </li>\r\n  <li>\r\n\r\n    <p class=\"bold underline\">\r\n      DISCLOSURE, STORAGE, AND ACCESS REQUESTS\r\n    </p>\r\n\r\n    <ol type=\"a\">\r\n      <li>\r\n        <strong>Retention of PharmaNet Data.</strong> You will not store or retain PharmaNet Data in\r\n        any paper files or any electronic system, unless such storage or retention is\r\n        required for record keeping in accordance with the Act, the Provider Regulation,\r\n        and Professional College requirements and in connection with your provision of\r\n        Health Services and otherwise is in compliance with the Conformance Standards.\r\n        You will not modify any records retained in accordance with this section other\r\n        than as may be expressly authorized in the Conformance Standards. For clarity,\r\n        you may annotate a discrete record provided that the discrete record is not itself\r\n        modified other than as expressly authorized in the Conformance Standards.\r\n      </li>\r\n      <li>\r\n        <strong>Disclosure to Third Parties.</strong> You will not disclose PharmaNet Data to any\r\n        Unauthorized Person, unless disclosure is required for Health Services or is\r\n        otherwise authorized under section 24(1) of the Act.\r\n      </li>\r\n      <li>\r\n        <strong>Responding to Patient Access Requests.</strong> Aside from any records retained by you\r\n        in accordance with section 6(a) of this Agreement, you will not provide to patients\r\n        any copies of records containing PharmaNet Data or “print outs” produced\r\n        directly from PharmaNet, and will refer any requests for access to such records or\r\n        “print outs” to the Province.\r\n      </li>\r\n      <li>\r\n        <strong>Responding to Requests to Correct a Record Contained in PharmaNet.</strong> If you\r\n        receive a request for correction of any record or information contained in\r\n        PharmaNet that can not be completed at the pharmacy, you will refer the request\r\n        to the Province.\r\n      </li>\r\n      <li>\r\n        <strong>Legal Demands for Records Contained in PharmaNet.</strong> You will immediately\r\n        notify the Province if you receive any order, demand or request compelling, or\r\n        threatening to compel, disclosure of records contained in PharmaNet. You will\r\n        cooperate and consult with the Province in responding to any such demands. For greater certainty, the foregoing\r\n        requires that you notify the Province only with\r\n        respect to any access requests or demands for records contained in PharmaNet,\r\n        and not records retained by you in accordance with section 6(a) of this\r\n        Agreement.\r\n      </li>\r\n    </ol>\r\n\r\n  </li>\r\n  <li>\r\n\r\n    <p class=\"bold underline\">\r\n      ACCURACY\r\n    </p>\r\n\r\n    <p>\r\n      You will make reasonable efforts to ensure that any Personal Information recorded by\r\n      you in PharmaNet is accurate, complete and up to date. In the event that you become\r\n      aware of a material inaccuracy or error in such information, you will take reasonable\r\n      steps to investigate the inaccuracy or error, correct it if necessary, and notify the Province\r\n      of the inaccuracy or error and any steps taken.\r\n    </p>\r\n\r\n  </li>\r\n\r\n  <li>\r\n\r\n    <p class=\"bold underline\">\r\n      NOTICE OF NON COMPLIANCE AND DUTY TO INVESTIGATE\r\n    </p>\r\n\r\n    <ol type=\"a\">\r\n      <li>\r\n\r\n        <p>\r\n          <strong>Non-Compliance.</strong> You will promptly notify the Province, and provide\r\n          particulars, if:\r\n        </p>\r\n\r\n        <ol type=\"i\">\r\n          <li>\r\n            you do not comply, or you anticipate that you will be unable to comply\r\n            with the terms of this Agreement in any respect, or\r\n          </li>\r\n          <li>\r\n            you have knowledge of any circumstances, incidents or events which have\r\n            or may jeopardize the security, confidentiality, or integrity of PharmaNet,\r\n            the provincial drug program, or any government network or electronic\r\n            system including any unauthorized attempt, by any person, to access\r\n            PharmaNet.\r\n          </li>\r\n        </ol>\r\n\r\n      </li>\r\n      <li>\r\n        <p>\r\n          <strong>Reports to College or Privacy Commissioner.</strong> You acknowledge that the\r\n          Province may report any material breach of the Act, the Information Management\r\n          Regulation, or these terms to your Professional College or to the Information and\r\n          Privacy Commissioner of British Columbia.\r\n        </p>\r\n      </li>\r\n    </ol>\r\n\r\n  </li>\r\n  <li>\r\n\r\n    <p class=\"bold underline\">\r\n      TERM OF AGREEMENT, SUSPENSION & TERMINATION\r\n    </p>\r\n\r\n    <ol type=\"a\">\r\n      <li>\r\n        <strong>Term.</strong> The term of this Agreement begins on the date you are granted access to\r\n        PharmaNet by the Province and will continue until the date this Agreement is\r\n        terminated under paragraph (b), (c), (d) or (e) below.\r\n      </li>\r\n      <li>\r\n        <strong>Termination for Any Reason.</strong> You may terminate this Agreement at any time on\r\n        written notice to the Province.\r\n      </li>\r\n      <li>\r\n        <strong>Suspension or Termination of PharmaNet Access.</strong> If the Province suspends or\r\n        terminates your right to access PharmaNet under the Information Management\r\n        Regulation, the Province may also terminate this Agreement at any time thereafter\r\n        upon written notice to you.\r\n      </li>\r\n      <li>\r\n        <strong>Termination for Breach.</strong> Notwithstanding paragraph (c) above, the Province\r\n        may terminate this Agreement immediately upon notice to you if you fail to\r\n        comply with any provision of this Agreement.\r\n      </li>\r\n      <li>\r\n        <strong>Termination by Operation of the Information Management Regulation.</strong> This\r\n        Agreement will terminate automatically if your access to PharmaNet ends by\r\n        operation of section 39 or 40 of the Information Management Regulation.\r\n      </li>\r\n      <li>\r\n        <strong>Suspension of Account for Inactivity.</strong> As a security precaution, the Province\r\n        may suspend your account after a period of inactivity, in accordance with the\r\n        Province’s policies. Please contact the Province immediately if your account has\r\n        been suspended for inactivity but you still require access to PharmaNet.\r\n      </li>\r\n    </ol>\r\n\r\n  </li>\r\n  <li>\r\n\r\n    <p class=\"bold underline\">\r\n      DISCLAIMER OF WARRANTY, LIMITATION OF LIABILITY AND INDEMNITY\r\n    </p>\r\n\r\n    <ol type=\"a\">\r\n      <li>\r\n        <strong>Information Provided As Is.</strong> You acknowledge and agree that any use of\r\n        PharmaNet and PharmaNet Data is solely at your own risk. All such access and\r\n        information is provided on an “as is” and “as available” basis without warranty or\r\n        condition of any kind. The Province does not warrant the accuracy, completeness\r\n        or reliability of the PharmaNet Data or the availability of PharmaNet, or that\r\n        access to or the operation of PharmaNet will function without error, failure or\r\n        interruption.\r\n      </li>\r\n      <li>\r\n        <strong>You Are Responsible.</strong> You are responsible for verifying the accuracy of\r\n        information disclosed to you as a result of your access to PharmaNet or otherwise\r\n        pursuant to this Agreement before relying or acting upon such information. The\r\n        clinical or other information disclosed to you pursuant to this Agreement is in no\r\n        way intended to be a substitute for professional judgment.\r\n      </li>\r\n      <li>\r\n        <strong>The Province Not Liable for Loss.</strong> No action may be brought by any person\r\n        against the Province for any loss or damage of any kind caused by any reason or\r\n        purpose related to reliance on PharmaNet or PharmaNet Data.\r\n      </li>\r\n      <li>\r\n        <strong>You Must Indemnify the Province If You Cause a Loss or Claim.</strong> You agree\r\n        to indemnify and save harmless the Province, and the Province’s employees and\r\n        agents (each an <strong>“Indemnified Person\"</strong>) from any losses, claims, damages,\r\n        actions, causes of action, costs and expenses that an Indemnified Person may\r\n        sustain, incur, suffer or be put to at any time, either before or after this Agreement\r\n        ends, which are based upon, arise out of or occur directly or indirectly by reason\r\n        of any act or omission by you in connection with this Agreement or in connection\r\n        with access to PharmaNet by you.\r\n      </li>\r\n    </ol>\r\n\r\n  </li>\r\n  <li>\r\n\r\n    <p class=\"bold underline\">\r\n      NOTICE\r\n    </p>\r\n\r\n    <ol type=\"a\">\r\n      <li>\r\n\r\n        <p>\r\n          <strong>Notice to Province.</strong> Except where this Agreement expressly provides for another\r\n          method of delivery, any notice to be given by you to the Province that is\r\n          contemplated by this Agreement, to be effective, must be in writing and emailed\r\n          or mailed to:\r\n        </p>\r\n\r\n        <address>\r\n          Director, Information and PharmaNet Development<br>\r\n          Ministry of Health<br>\r\n          PO Box 9652, STN PROV GOVT<br>\r\n          Victoria, BC V8W 9P4<br>\r\n\r\n          <br>\r\n\r\n          <a href=\"mailto:PRIMESupport@gov.bc.ca\">PRIMESupport@gov.bc.ca</a>\r\n        </address>\r\n\r\n      </li>\r\n      <li>\r\n        <strong>Notice to You.</strong> Any notice to you to be delivered under the terms of this\r\n        Agreement will be in writing and delivered by the Province to you using any of\r\n        the contact mechanisms identified by you in PRIME, including by mail to a\r\n        specified postal address, email to a specified email address or text message to the\r\n        specified cell phone number. You may be required to click a URL link or log into\r\n        PRIME to receive the content of any such notice.\r\n      </li>\r\n      <li>\r\n        <strong>Deemed Receipt.</strong> Any written communication from a party, if personally\r\n        delivered or sent electronically, will be deemed to have been received 24 hours\r\n        after the time the notice was sent, or, if sent by mail, will be deemed to have been\r\n        received 3 days (excluding Saturdays, Sundays and statutory holidays) after the\r\n        date the notice was sent.\r\n      </li>\r\n      <li>\r\n        <strong>Substitute Contact Information.</strong> You may notify the Province of a substitute\r\n        contact mechanism by updating your contact information in PRIME.\r\n      </li>\r\n    </ol>\r\n\r\n  </li>\r\n  <li>\r\n\r\n    <p class=\"bold underline\">\r\n      GENERAL\r\n    </p>\r\n\r\n    <ol type=\"a\">\r\n      <li>\r\n\r\n        <p>\r\n          <strong>Severability.</strong> Each provision in this Agreement constitutes a separate covenant\r\n          and is severable from any other covenant, and if any of them are held by a court,\r\n          or other decision-maker, to be invalid, this Agreement will be interpreted as if\r\n          such provisions were not included.\r\n        </p>\r\n\r\n      </li>\r\n      <li>\r\n\r\n        <p>\r\n          <strong>Survival.</strong> Any provision of this Agreement that expressly or by its nature\r\n          continues after termination, shall survive termination of this Agreement.\r\n        </p>\r\n\r\n      </li>\r\n      <li>\r\n\r\n        <p>\r\n          <strong>Governing Law.</strong> This Agreement will be governed by and will be construed and\r\n          interpreted in accordance with the laws of British Columbia and the laws of\r\n          Canada applicable therein.\r\n        </p>\r\n\r\n      </li>\r\n      <li>\r\n\r\n        <p>\r\n          <strong>Assignment Restricted.</strong> Your rights and obligations under this Agreement may\r\n          not be assigned without the prior written approval of the Province.\r\n        </p>\r\n\r\n      </li>\r\n      <li>\r\n\r\n        <p>\r\n          <strong>Waiver.</strong> The failure of the Province at any time to insist on performance of any\r\n          provision of this Agreement by you is not a waiver of its right subsequently to\r\n          insist on performance of that or any other provision of this Agreement.\r\n        </p>\r\n\r\n      </li>\r\n      <li>\r\n\r\n        <p>\r\n          <strong>Province May Modify this Agreement.</strong> The Province may amend this\r\n          Agreement, including this section, at any time in its sole discretion:\r\n        </p>\r\n\r\n        <ol type=\"i\">\r\n          <li>\r\n            by written notice to you, in which case the amendment will become\r\n            effective upon the later of (A) the date notice of the amendment is first\r\n            delivered to you, or (B) the effective date of the amendment specified by\r\n            the Province, if any; or\r\n          </li>\r\n          <li>\r\n            by publishing notice of any such amendment in the PharmaCare\r\n            Newsletter, in which case the notice will specify the effective date of the\r\n            amendment, which date will be at least 30 (thirty) days after the date that\r\n            the PharmaCare Newsletter containing the notice is first published.\r\n          </li>\r\n        </ol>\r\n\r\n        <p>\r\n          If you use PharmaNet after the effective date of an amendment described in (i) or\r\n          (ii) above, you will be deemed to have accepted the corresponding amendment,\r\n          and this Agreement will be deemed to have been so amended as of the effective\r\n          date. If you do not agree with any amendment for which notice has been provided\r\n          by the Province in accordance with (i) or (ii) above, you must promptly (and in\r\n          any event before the effective date) cease all access or use of PharmaNet by yourself and take the steps\r\n          necessary to terminate this Agreement in accordance\r\n          with section 10.\r\n        </p>\r\n\r\n      </li>\r\n    </ol>\r\n\r\n  </li>\r\n</ol>\r\n",
                (int)AgreementType.PharmacyTechnicianTOA, DateTimeOffset.Parse("2019-09-16 -7:00") });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AgreementVersion",
                keyColumn: "Id",
                keyValue: 18);
        }
    }
}
